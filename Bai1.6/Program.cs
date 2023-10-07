using System;
using System.IO;


class Mamal
{
    private string characteristics;
    public string Characteristics
    {
        get { return characteristics; } 
        set { characteristics = value; }
    }
}
class Whale : Mamal
{
    public Whale()
    {
        Console.WriteLine("This is a whale");
    }
}
class Human : Mamal, IAbility
{
    public Human()
    {
        Console.WriteLine("This is a human");
    }
    public void thinking_behaviour()
    {
        Console.WriteLine("I'm thinking");
    }
    public void intelligent_behaviour()
    {
        Console.WriteLine("I'm doing intelligent things");
    }
}

interface IThinking
{
    void thinking_behaviour();  
}
interface IIntelligent
{
    void intelligent_behaviour();
}

interface IAbility : IThinking, IIntelligent
{

}
class Program
{
    public static void Main(string[] args)
    {
        Whale whale = new Whale();
        Human human = new Human();
        human.thinking_behaviour();
        human.intelligent_behaviour();
    }
}