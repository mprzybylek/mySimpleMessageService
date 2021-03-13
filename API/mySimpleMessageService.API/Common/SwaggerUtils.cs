using Microsoft.OpenApi.Models;

namespace mySimpleMessageService.API.Common
{
    public static class SwaggerUtils
    {
        public static OpenApiInfo GetSwaggerHeader()
        {
            return new OpenApiInfo
            {
                Title = "mySimpleMessageService",
                Version = "v1",
                Description = "Elements of system : \n" +
                        "- API\n" +
                        "- Domain ( logic of application\n" +
                        "- Persistance ( db layer )\n" +
                        "\n" +
                        "Requirements:\n" +
                        "\nMessages \n" +
                        "- Send a message to another contact within the engine.\n" +
                        "- Read messages \n" +
                        "- Delete a message\n" +
                        "- Apply filtering, pagination and sorting \n" +
                        "Sample sorting: GET /api/Contacts?$orderby=name%20desc \n" +
                        "Sample filtering: GET /api/Contacts?$filter=name%20eq%20%27Mateusz%27 \n" +
                        "Sample pagination: GET /api/Contacts?$skip=1&$top=1" +
                        "\nContacts \n" +
                        "- CRUD operations\n" +
                        "\nOthers:\n" +
                        "- Code reusable\n" +
                        "- Testable code\n" +
                        "- Ready for improvements\n"
            };
        }
    }
}
