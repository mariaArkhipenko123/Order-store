using HotChocolate.Types;
using Lab.Domain.Entities;

namespace Lab.Application.Models.DTOs.GraphQL
{
    public class UserType : ObjectType<User>
    {
        protected override void Configure(IObjectTypeDescriptor<User> descriptor)
        {
            descriptor.Field(u => u.CreatedAt).Ignore(); 
            descriptor.Field(u => u.UpdatedAt).Ignore();  
        }
    }
}
