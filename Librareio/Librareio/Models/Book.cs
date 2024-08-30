using System;
using System.Collections.Generic;

namespace Librareio.Models;

public partial class Book
{
    public int SubmissionId { get; set; }

    public byte[]? DisplayImage { get; set; }

    public string? BookName { get; set; }

    public string? Author { get; set; }

    public string? Description { get; set; }

    public DateTime? SubmissionDate { get; set; }

    public byte[]? PdfFile { get; set; }
}
