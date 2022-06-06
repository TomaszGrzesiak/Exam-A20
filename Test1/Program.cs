// See https://aka.ms/new-console-template for more information

using System;
using Domain.Models;

Author aaa = new Author()
{
    FirstName = "1",
    LastName = "2",
    Books = new []
    {
        new Book() {ISBN = 1},
        new Book() {ISBN = 2},
        new Book() {ISBN = 3}
    }
};

Console.WriteLine(aaa.ToString());