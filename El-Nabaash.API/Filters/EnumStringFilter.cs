using El_Nabaash.API.Enums;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace El_Nabaash.API.Filters;

public class EnumStringFilter : ISchemaFilter
{
    public void Apply(IOpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type == typeof(string) && context.MemberInfo?.Name == "Type")
        {
            schema.Description =
                "Allowed values : " + string.Join(", ", Enum.GetNames(typeof(ArtifactType)));
        }
    }
}