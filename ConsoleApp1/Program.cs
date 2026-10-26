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

        public virtual float Circumference()
        {
            float circ = 2*Side1 + 2*Side2;
            Console.WriteLine($"Rectangle circumference: {circ}");
            return circ;
        }

        public virtual float Area()
        {
            float area = Side1 * Side2;
            Console.WriteLine($"Rectangle area: {area}");
            return area;
        }


    }

    class Square : Rectangle
    {
        private float Side { get; set; }
        public Square(float side) : base(side, side) 
        {
            Side = side;
        }

        public override float Circumference()
        {
            float circ = 4 * Side;
            Console.WriteLine($"Square circumference: {circ}");
            return circ;
        }

        public override float Area()
        {
            float area = Side * Side;
            Console.WriteLine($"Square Area: {area}");
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
            Console.WriteLine($"Hi, I'm {Name} and I'm a {Age} years old animal.");
        }
    }

    class Cat : Animal
    {


        public Cat(string name, int age) : base(name, age)
        { }
        

        public override void Speak()
        {
            Console.WriteLine($"Hi, I'm {Name} and I'm a {Age} years old cat. Meow!");
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
            someCat.Birthday();
            someCat.WhatAge();

            Rectangle rectangle1 = new Rectangle(12, 5);
            rectangle1.Circumference();
            rectangle1.Area();

            Square square1 = new Square(5);
            square1.Area();
            square1.Circumference();

        }
    }
}
