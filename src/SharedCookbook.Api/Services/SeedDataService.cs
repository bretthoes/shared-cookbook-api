using SharedCookbook.Api.Data;
using SharedCookbook.Api.Data.Entities;

namespace SharedCookbook.Api.Services;

public class SeedDataService : ISeedDataService
{
    public void Initialize(SharedCookbookContext context)
    {
        // seed if person table is empty
        if (!context.People.Any())
        {
            SeedData(context);
        }

        return;
    }

    private void SeedData(SharedCookbookContext context)
    {
        var people = new List<Person>
        {
            new() {
                PersonId = 0,
                DisplayName = "Brett",
                Email = "bretthoes@gmail.com",
                ImagePath = null,
                PasswordHash = "9389F2261E8E167A17C8F22224FAA22F5DB5F717447E550CE0480EE1286B392A:4B3E39CC74BABB68BEF214C931406235:50000:SHA256"
            },
            new() {
                PersonId = 0,
                DisplayName = "Test",
                Email = "test@test.com",
                ImagePath = null,
                PasswordHash = "9389F2261E8E167A17C8F22224FAA22F5DB5F717447E550CE0480EE1286B392A:4B3E39CC74BABB68BEF214C931406235:50000:SHA256"
            }
        };

        var cookbooks = new List<Cookbook>
        {
            new() {
                CookbookId = 0,
                Title = "Brett's Cookbook",
                Creator = people.First(),
            },
            //new() {
            //    CookbookId = 0,
            //    Title = "Brett's 2nd Cookbook",
            //    Creator = people.First(),
            //},
            //new() {
            //    CookbookId = 0,
            //    Title = "Brett's 3nd Cookbook",
            //    Creator = people.First(),
            //},
            //new() {
            //    CookbookId = 0,
            //    Title = "Brett's 4th Cookbook",
            //    Creator = people.First(),
            //},
            //new() {
            //    CookbookId = 0,
            //    Title = "Brett's 5th Cookbook",
            //    Creator = people.First(),
            //},
            //new() {
            //    CookbookId = 0,
            //    Title = "Brett's 6th Cookbook",
            //    Creator = people.First(),
            //},
            new() {
                CookbookId = 0,
                Title = "Test2's Cookbook",
                Creator = new() {
                PersonId = 0,
                DisplayName = "Test2",
                Email = "test2@test.com",
                ImagePath = null,
                PasswordHash = "9389F2261E8E167A17C8F22224FAA22F5DB5F717447E550CE0480EE1286B392A:4B3E39CC74BABB68BEF214C931406235:50000:SHA256"
            },
            }
        };

        var members = new List<CookbookMember>
        {
            new() {
                CookbookMemberId = 0,
                Person = people[0],
                Cookbook = cookbooks[0],
                CanAddRecipe = true,
                CanDeleteRecipe = true,
                CanEditCookbookDetails = true,
                CanRemoveMember = true,
                CanSendInvite = true,
                CanUpdateRecipe = true,
                PersonId = 0,
                CookbookId = 0,
                JoinDate = DateTime.Now
            },
            new() {
                CookbookMemberId = 0,
                Person = people[0],
                Cookbook = cookbooks[1],
                CanAddRecipe = true,
                CanDeleteRecipe = true,
                CanEditCookbookDetails = true,
                CanRemoveMember = true,
                CanSendInvite = true,
                CanUpdateRecipe = true,
                PersonId = 0,
                CookbookId = 0,
                JoinDate = DateTime.Now
            },
            //new() {
            //    CookbookMemberId = 0,
            //    Person = people[0],
            //    Cookbook = cookbooks[2],
            //    CanAddRecipe = true,
            //    CanDeleteRecipe = true,
            //    CanEditCookbookDetails = true,
            //    CanRemoveMember = true,
            //    CanSendInvite = true,
            //    CanUpdateRecipe = true,
            //    PersonId = 0,
            //    CookbookId = 0,
            //    JoinDate = DateTime.Now
            //},
            //new() {
            //    CookbookMemberId = 0,
            //    Person = people[0],
            //    Cookbook = cookbooks[3],
            //    CanAddRecipe = true,
            //    CanDeleteRecipe = true,
            //    CanEditCookbookDetails = true,
            //    CanRemoveMember = true,
            //    CanSendInvite = true,
            //    CanUpdateRecipe = true,
            //    PersonId = 0,
            //    CookbookId = 0,
            //    JoinDate = DateTime.Now
            //},
            //new() {
            //    CookbookMemberId = 0,
            //    Person = people[0],
            //    Cookbook = cookbooks[4],
            //    CanAddRecipe = true,
            //    CanDeleteRecipe = true,
            //    CanEditCookbookDetails = true,
            //    CanRemoveMember = true,
            //    CanSendInvite = true,
            //    CanUpdateRecipe = true,
            //    PersonId = 0,
            //    CookbookId = 0,
            //    JoinDate = DateTime.Now
            //},
            //new() {
            //    CookbookMemberId = 0,
            //    Person = people[0],
            //    Cookbook = cookbooks[5],
            //    CanAddRecipe = true,
            //    CanDeleteRecipe = true,
            //    CanEditCookbookDetails = true,
            //    CanRemoveMember = true,
            //    CanSendInvite = true,
            //    CanUpdateRecipe = true,
            //    PersonId = 0,
            //    CookbookId = 0,
            //    JoinDate = DateTime.Now
            //},
            //new() {
            //    CookbookMemberId = 0,
            //    Person = people[1],
            //    Cookbook = cookbooks[6],
            //    CanAddRecipe = true,
            //    CanDeleteRecipe = true,
            //    CanEditCookbookDetails = true,
            //    CanRemoveMember = true,
            //    CanSendInvite = true,
            //    CanUpdateRecipe = true,
            //    PersonId = 0,
            //    CookbookId = 0,
            //    JoinDate = DateTime.Now
            //}
        };

        var recipes = new List<Recipe>
        {
            new()
            {
                RecipeId = 0,
                CookbookId = 0,
                Title = "Chicken Casserole",
                Cookbook = cookbooks[0],
                Author = people[0],
                ImagePath = "assets/images/chicken_casserole.png",
                PreparationTimeInMinutes = 30,
                CookingTimeInMinutes = 30,
            },
            new()
            {
                RecipeId = 0,
                CookbookId = 0,
                Title = "Salmon Loaf",
                Cookbook = cookbooks[0],
                Author = people[1],
                ImagePath = "assets/images/chicken_casserole.png",
                BakingTimeInMinutes = 90
            }
        };

        context.People.AddRange(people);
        context.Cookbooks.AddRange(cookbooks);
        context.CookbookMembers.AddRange(members);
        context.Recipes.AddRange(recipes);
    }
}

