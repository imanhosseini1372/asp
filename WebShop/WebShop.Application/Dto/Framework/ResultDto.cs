using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShop.Application.Dto.Users;

namespace WebShop.Application.Dto.Framework
{
    public class ResultDto<T>
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public T Result { get; set; }
    }
    public class ResultDto
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    
    }
    
}
