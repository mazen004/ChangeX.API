    using AutoMapper;
using ChangeX.BLL.DTOs;
using ChangeX.BLL.Interfaces;
using ChangeX.DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Linq.Expressions;

namespace ChangeX.API.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DetailController(IDetailServices detailServices, IMapper mapper, IWebHostEnvironment environment,ICurrentUserService currentUser) : ControllerBase
    {
        private const long MaxAttachmentSizeInBytes = 10 * 1024 * 1024;
        private const long MaxUploadRequestSizeInBytes = MaxAttachmentSizeInBytes + (1024 * 1024);

        private static readonly Dictionary<string, HashSet<string>> AllowedAttachmentTypes =
            new(StringComparer.OrdinalIgnoreCase)
            {
                [".pdf"] = new(StringComparer.OrdinalIgnoreCase) { "application/pdf" },
                [".doc"] = new(StringComparer.OrdinalIgnoreCase) { "application/msword" },
                [".docx"] = new(StringComparer.OrdinalIgnoreCase)
                {
                    "application/vnd.openxmlformats-officedocument.wordprocessingml.document"
                },
                [".xls"] = new(StringComparer.OrdinalIgnoreCase) { "application/vnd.ms-excel" },
                [".xlsx"] = new(StringComparer.OrdinalIgnoreCase)
                {
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                },
                [".jpg"] = new(StringComparer.OrdinalIgnoreCase) { "image/jpeg", "image/pjpeg" },
                [".jpeg"] = new(StringComparer.OrdinalIgnoreCase) { "image/jpeg", "image/pjpeg" },
                [".png"] = new(StringComparer.OrdinalIgnoreCase) { "image/png" }
            };

        [HttpGet]
        public async Task<IActionResult> GetDetails(
            [FromQuery] Guid? crId,
            [FromQuery] string? state)
        {
            Expression<Func<Detail, bool>>? predicate = null;

            if (crId.HasValue || !string.IsNullOrWhiteSpace(state))
            {
                predicate = detail =>
                    (!crId.HasValue || detail.CRID == crId.Value) &&
                    (string.IsNullOrWhiteSpace(state) || detail.State == state);
            }

            var result = await detailServices.GetAll(predicate);

            if (!result.Success)
            {
                return StatusCode(result.StatusCode, new { message = result.Message });
            }

            return Ok(new { message = result.Message, data = result.Data });
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetDetailById(Guid id)
        {
            var result = await detailServices.GetByID(id);
            if (!result.Success)
            {
                return StatusCode(result.StatusCode, new { message = result.Message });
            }

            return Ok(new { message = result.Message, data = result.Data });
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        [RequestSizeLimit(MaxUploadRequestSizeInBytes)]
        [RequestFormLimits(MultipartBodyLengthLimit = MaxUploadRequestSizeInBytes)]
        public async Task<IActionResult> CreateDetail([FromForm] DetailDto detailDto)
        {
            if (detailDto.Attachment == null || detailDto.Attachment.Length == 0)
            {
                return BadRequest(new { message = "Attachment is required." });
            }

            var attachmentValidationError = ValidateAttachment(detailDto.Attachment);
            if (attachmentValidationError != null)
            {
                return BadRequest(new { message = attachmentValidationError });
            }

            var detail = mapper.Map<Detail>(detailDto);
            detail.Attachment = await SaveAttachment(detailDto.Attachment);

            ServiceResponse<Detail> result;
            try
            {
                result = await detailServices.Create(detail);
            }
            catch
            {
                DeleteAttachment(detail.Attachment);
                throw;
            }

            if (!result.Success)
            {
                DeleteAttachment(detail.Attachment);
                return StatusCode(result.StatusCode, new { message = result.Message });
            }

            return StatusCode(
                StatusCodes.Status201Created,
                new { message = result.Message, data = result.Data });
        }

        [HttpPut("{id:guid}")]
        [Consumes("multipart/form-data")]
        [RequestSizeLimit(MaxUploadRequestSizeInBytes)]
        [RequestFormLimits(MultipartBodyLengthLimit = MaxUploadRequestSizeInBytes)]
        public async Task<IActionResult> UpdateDetail(Guid id, [FromForm] DetailDto detailDto)
        {
            if (detailDto.Attachment != null && detailDto.Attachment.Length > 0)
            {
                var attachmentValidationError = ValidateAttachment(detailDto.Attachment);
                if (attachmentValidationError != null)
                {
                    return BadRequest(new { message = attachmentValidationError });
                }
            }

            var getResult = await detailServices.GetByID(id);
            if (!getResult.Success)
            {
                return StatusCode(getResult.StatusCode, new { message = getResult.Message });
            }

            var oldAttachment = getResult.Data!.Attachment;
            mapper.Map(detailDto, getResult.Data);

            string? newAttachment = null;
            if (detailDto.Attachment != null && detailDto.Attachment.Length > 0)
            {
                newAttachment = await SaveAttachment(detailDto.Attachment);
                getResult.Data.Attachment = newAttachment;
            }

            ServiceResponse<Detail> result;
            try
            {
                result = await detailServices.Update(getResult.Data);
            }
            catch
            {
                if (newAttachment != null)
                {
                    DeleteAttachment(newAttachment);
                }

                throw;
            }

            if (!result.Success)
            {
                if (newAttachment != null)
                {
                    DeleteAttachment(newAttachment);
                }

                return StatusCode(result.StatusCode, new { message = result.Message });
            }

            if (newAttachment != null)
            {
                DeleteAttachment(oldAttachment);
            }

            return Ok(new { message = result.Message, data = result.Data });
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteDetail(Guid id)
        {
            var getResult = await detailServices.GetByID(id);
            if (!getResult.Success)
            {
                return StatusCode(getResult.StatusCode, new { message = getResult.Message });
            }

            var result = await detailServices.Delete(id);
            if (!result.Success)
            {
                return StatusCode(result.StatusCode, new { message = result.Message });
            }

            DeleteAttachment(getResult.Data!.Attachment);

            return Ok(new { message = result.Message });
        }

        private static string? ValidateAttachment(IFormFile attachment)
        {
            if (attachment.Length > MaxAttachmentSizeInBytes)
            {
                return "Attachment size must not exceed 10 MB.";
            }

            var extension = Path.GetExtension(attachment.FileName);
            if (string.IsNullOrWhiteSpace(extension) ||
                !AllowedAttachmentTypes.TryGetValue(extension, out var allowedContentTypes))
            {
                return "Invalid attachment type. Allowed types: PDF, Word, Excel, JPG, JPEG, and PNG.";
            }

            if (!allowedContentTypes.Contains(attachment.ContentType))
            {
                return "Attachment content type does not match its file extension.";
            }

            return null;
        }

        private async Task<string> SaveAttachment(IFormFile attachment)
        {
            var extension = Path.GetExtension(attachment.FileName);
            var fileName = $"{Guid.NewGuid()}{extension}";
            var folderPath = GetAttachmentFolderPath();

            Directory.CreateDirectory(folderPath);

            var filePath = Path.Combine(folderPath, fileName);
            await using var stream = new FileStream(filePath, FileMode.Create);
            await attachment.CopyToAsync(stream);

            return $"/attachments/details/{fileName}";
        }

        private void DeleteAttachment(string attachmentPath)
        {
            if (string.IsNullOrWhiteSpace(attachmentPath))
            {
                return;
            }

            var fileName = Path.GetFileName(attachmentPath);
            var filePath = Path.Combine(GetAttachmentFolderPath(), fileName);

            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
        }

        private string GetAttachmentFolderPath()
        {
            var webRootPath = environment.WebRootPath
                ?? Path.Combine(environment.ContentRootPath, "wwwroot");

            return Path.Combine(webRootPath, "attachments", "details");
        }
    }
}
