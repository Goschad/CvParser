document.addEventListener("DOMContentLoaded", function ()
{
    const input = document.getElementById("fileInput");
    const fileText = document.getElementById("fileText");
    const defaultText = "Drop your PDF here or click to upload (max 5 MB)";

    if (!input || !fileText) return;

    input.addEventListener("change", function ()
    {
        const file = this.files && this.files[0];

        if (!file)
        {
            fileText.textContent = defaultText;
            return;
        }

        const isPdf =
            file.type === "application/pdf" ||
            file.name.toLowerCase().endsWith(".pdf");

        const maxSize = 5 * 1024 * 1024;

        if (!isPdf)
        {
            alert("Only PDF files are allowed.");
            input.value = "";
            fileText.textContent = defaultText;
            return;
        }

        if (file.size > maxSize)
        {
            alert("The file must not exceed 5 MB.");
            input.value = "";
            fileText.textContent = defaultText;
            return;
        }

        fileText.textContent = file.name;
    });
});