using SharedCookbookApi.Data;
using SharedCookbookApi.Data.Entities;
using System;

namespace SharedCookbookApi.Services;

public class SeedDataService : ISeedDataService
{
    public void Initialize(SharedCookbookContext context)
    {
        // seed if person table is empty
        if (context.People.Count() == 0) 
        {
            SeedData(context);
        }

        return;
    }

    private void SeedData(SharedCookbookContext context)
    {
        var people = new List<Person> 
        { 
            new Person
            {
                PersonId = 0,
                DisplayName = "Brett",
                Email = "bretthoes@gmail.com",
                ImagePath = "/images/image.jpg",
                PasswordHash = "password123",
                PasswordSalt = "password_salt",
                FirebaseUserUid = "firebase_user_uid",
            }
        };

        var cookbooks = new List<Cookbook>
        {
            new Cookbook
            {
                CookbookId = 0,
                Title = "Brett's Cookbook",
                Creator = people.First(),

            }
        };

        var members = new List<CookbookMember>
        {
            new CookbookMember
            {
                CookbookMemberId = 0,
                Person = people.First(),
                Cookbook = cookbooks.First(),
                CanAddRecipe = true,
                CanDeleteRecipe = true,
                CanEditCookbookDetails = true,
                CanRemoveMember = true,
                CanSendInvite = true,
                CanUpdateRecipe = true,
                PersonId = 0,
                CookbookId = 0,
                JoinDate = DateTime.Now
            }
        };
        context.People.AddRange(people);
        context.Cookbooks.AddRange(cookbooks);
        context.CookbookMembers.AddRange(members);
    }
}

