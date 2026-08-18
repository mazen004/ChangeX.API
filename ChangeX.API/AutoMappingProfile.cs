using AutoMapper;
using ChangeX.BLL.DTOs;
using ChangeX.DAL.Entities;

namespace ChangeX.API
{
    public sealed class AutoMappingProfile : Profile
    {
        public AutoMappingProfile()
        {
            var entityTypes = typeof(Client).Assembly
                .GetTypes()
                .Where(type =>
                    type.IsClass &&
                    !type.IsAbstract &&
                    type.Namespace == typeof(Client).Namespace)
                .ToArray();

            var dtoTypes = typeof(ClientDto).Assembly
                .GetTypes()
                .Where(type =>
                    type.IsClass &&
                    !type.IsAbstract &&
                    type.Name.EndsWith("Dto", StringComparison.Ordinal))
                .ToArray();

            foreach (var dtoType in dtoTypes)
            {
                var entityType = FindEntityType(dtoType, entityTypes);
                if (entityType is not null)
                {
                    CreateMap(entityType, dtoType).ReverseMap();
                }
            }
        }

        private static Type? FindEntityType(Type dtoType, IEnumerable<Type> entityTypes)
        {
            var dtoName = dtoType.Name[..^"Dto".Length];
            var entities = entityTypes
                .OrderByDescending(type => type.Name.Length)
                .ToArray();

            return entities.FirstOrDefault(entity => entity.Name == dtoName)
                ?? entities.FirstOrDefault(entity => dtoName.StartsWith(
                    entity.Name,
                    StringComparison.Ordinal))
                ?? entities.FirstOrDefault(entity => dtoName.EndsWith(
                    entity.Name,
                    StringComparison.Ordinal));
        }
    }
}
