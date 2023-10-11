using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3ai.solutions.BrevoWrapper.Models
{
    public record Result
    {
        public bool Success { get; init; }
        public string? Message { get; init; }
        public string? MessageId { get; init; }
        public Exception? Exception { get; init; }
    }
}
