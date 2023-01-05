@using Microsoft.AspNetCore.Mvc.RazorPages
@using Microsoft.AspNetCore.Mvc.Rendering

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>ViewData["Title"] - gabinet_rejestracja</title>
    <link rel="stylesheet" href="wwwroot/lib/bootstrap/dist/css/bootstrap.min.css" />
    <link rel="stylesheet" href="wwwroot/css/site.css" asp-append-version="true" />
</head>
<body>
    <div class="container">
        <main role="main" class="pb-3">
            @RenderPage("Views/Home/Index.cshtml")
        </main>
    </div>
    <script src="wwwroot/lib/jquery/dist/jquery.min.js"></script>
    <script src="wwwroot/lib/bootstrap/dist/js/bootstrap.bundle.min.js"></script>
    <script src="wwwroot/js/site.js" asp-append-version="true"></script>
    @await RenderSectionAsync("Scripts", required: false)
</body>
</html>
