using Configuration.API.Files;
using Configuration.API.Routing;
using Enterprise.API.Controllers.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace Configuration.API.Controllers;

[Route(RouteTemplates.Documentation)]
[ApiController]
public class DocumentationController : CustomControllerBase
{
    private readonly FileExtensionContentTypeProvider _fileExtensionContentTypeProvider;

    public DocumentationController(FileExtensionContentTypeProvider fileExtensionContentTypeProvider)
    {
        _fileExtensionContentTypeProvider = fileExtensionContentTypeProvider;
    }

    [HttpGet(Name = RouteNames.GetDocumentation)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetDocumentationFile()
    {
        string pathToFile = FileConstants.DocumentationFileName;

        if (!System.IO.File.Exists(pathToFile))
            return NotFound();

        string fileName = Path.GetFileName(pathToFile);
        byte[] fileBytes = System.IO.File.ReadAllBytes(pathToFile);

        if (!_fileExtensionContentTypeProvider.TryGetContentType(pathToFile, out string? contentType))
        {
            // default media type for arbitrary binary data
            contentType = FileConstants.DefaultFallbackContentType;
        }

        return File(fileBytes, contentType, fileName);
    }
}