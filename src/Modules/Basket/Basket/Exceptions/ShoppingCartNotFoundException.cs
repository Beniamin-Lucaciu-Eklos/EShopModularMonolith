using EShop.Shared.Exceptions;
using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Basket.Basket.Exceptions
{
    public class ShoppingCartNotFoundException(string userName) : NotFoundException
        ("ShoppingCart", userName)
    {
    }
}
