# CvParser

A web application written in F# that allows users to upload a PDF resume and automatically extract key information: first name, last name, email address, and phone number.

This project was built as a learning exercise to get familiar with F# and its ecosystem — functional patterns, type system, pipelines, and web development with Saturn and Giraffe.

## Tech stack

- **F#** — primary language
- **Saturn** — web framework (built on ASP.NET Core)
- **Giraffe / Giraffe.ViewEngine** — HTTP routing and HTML view generation in pure F#
- **PdfPig** — text extraction from PDF files
- **ASP.NET Core Session** — temporary storage of parsed data between requests

## Project structure

```
src/
├── Domain/
│   ├── Types.fs        — shared types (CvData)
│   ├── Heuristic.fs    — name and first name detection logic
│   └── Parser.fs       — email, phone extraction and parsing coordination
├── Api/
│   ├── Handlers.fs     — HTTP handlers (upload, parse)
│   └── Router.fs       — route definitions
├── Pages/
│   ├── Index.fs        — upload page
│   └── Result.fs       — result page
wwwroot/                — static assets (CSS, JS)
Program.fs              — entry point, application configuration
```

## Requirements

- [.NET SDK 8.x](https://dotnet.microsoft.com/download)

## Installation

```bash
git clone https://github.com/Goschad/CvParser.git
cd CvParser
dotnet restore
```

## Running the app

```bash
dotnet run
```

The application is available at `http://localhost:5078`.

## How it works

1. The user uploads a PDF file on the home page (maximum size: 5 MB).
2. Text is extracted page by page using PdfPig, reconstructing lines from the Y positions of words.
3. The parser applies a series of heuristics to identify the first name and last name — detecting fully uppercased words, capitalized words, and filtering out noise such as job titles or CV section headers.
4. Email and phone number are extracted using regular expressions.
5. Results are stored in session and displayed on the `/result` page.

## Known limitations

First name and last name extraction relies on heuristics. It handles most common cases for both French and English resumes, but may fail on atypical layouts or when a job title appears before the name at the top of the document.