namespace Pets.Models;

public class Pets
{
    public int Id { get; set; }
    public string Name { get; set; }
    public PetType Type { get; set; }
    public int ParentId { get; set; }
    public User Parent { get; set; }
}

public enum PetType
{
    Dog = 1,
    Cat = 2,
    Bird = 3,
    Pig = 4
}