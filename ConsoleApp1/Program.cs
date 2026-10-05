using System;
using System.Collections.Generic;

namespace ConsoleApp1
{
    abstract class Shape
    {
        public abstract float Circumference();


    }

    class Triangle : Shape
    {
        private float Side1 { get; }
        private float Side2 { get; }
        private float Side3 { get; }

        public Triangle(float side1, float side2, float side3)
        {
            Side1 = side1;
            Side2 = side2;
            Side3 = side3;
        }

        public override float Circumference()
        {
            float circ = Side1 + Side2 + Side3;
            Console.WriteLine($"Circumference: {circ}");
            return circ;
        }


    }
    class Animal
    {
        public virtual void Speak()
        {
            Console.WriteLine("Hi");
        }
    }

    class Cat : Animal
    {
        public string Name { get; }
        private int Age { get; }
        public Cat(string name, int age)
        {
            Name = name;
            Age = age;
        }

        public override void Speak()
        {
            Console.WriteLine($"Hi, I'm {Name} and I'm {Age} years old. Meow!");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Animal someAnimal = new Animal();
            someAnimal.Speak();

            Cat someCat = new Cat("Mimi", 5);
            someCat.Speak();

            Triangle triangle = new Triangle(12, 5, 4);
            triangle.Circumference();
        }
    }
}
