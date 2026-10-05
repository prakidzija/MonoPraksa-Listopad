using System;
using System.Collections.Generic;

namespace ConsoleApp1
{
    interface IShape
    {
        public float Circumference();

        public float Area();
    }

    class Rectangle : IShape
    {
        private float Side1 { get; }
        private float Side2 { get; }

        public Rectangle(float side1, float side2)
        {
            Side1 = side1;
            Side2 = side2;
        }

        public float Circumference()
        {
            float circ = 2*Side1 + 2*Side2;
            Console.WriteLine($"Circumference: {circ}");
            return circ;
        }

        public float Area()
        {
            float area = Side1 * Side2;
            Console.WriteLine($"Area: {area}");
            return area;
        }


    }
    class Animal
    {
        public string Name { get; set; }
        protected int Age { get; set; }

        public Animal(string name, int age)
        {
            Name = name;
            Age = age;
        }

        public void Birthday()
        {
            Age++;
        }

        public void WhatAge()
        {
            Console.WriteLine($"I'm {Age} years old");
        }

        public virtual void Speak()
        {
            Console.WriteLine("Hi");
        }
    }

    class Cat : Animal
    {


        public Cat(string name, int age) : base(name, age)
        { }
        

        public override void Speak()
        {
            Console.WriteLine($"Hi, I'm {Name} and I'm {Age} years old. Meow!");
        }

        
    }

    class Program
    {
        static void Main(string[] args)
        {
            Animal someAnimal = new Animal("Leo", 2);
            someAnimal.Speak();

            Cat someCat = new Cat("Mimi", 5);
            someCat.Speak();

            Rectangle rectangle1 = new Rectangle(12, 5);
            rectangle1.Circumference();
            rectangle1.Area();

            someCat.Birthday();
            someCat.WhatAge();

        }
    }
}
