using Data;
using Microsoft.Extensions.Configuration;

var project = new DBEFApp.Project();
project.Initialize();

var isAvailable = project.CheckDB();
if(!isAvailable)
{
    Console.WriteLine("Cannot connect to Database");
    System.Environment.Exit(1);
}

// project.CreateEnclosures();
project.AddBoryaAndManyaInEnc1();
project.PrintAnimals();
// project.ChangeBorya();


