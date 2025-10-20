using Blogging_Platform_Backend.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blogging_Platform_Backend.DTOModels.Comments
{
    public class dtoComment
    {
        public string Content { get; set; }
        public string PostId { get; set; }
    }
}
