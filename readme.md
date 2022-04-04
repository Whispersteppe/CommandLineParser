# Command Line Parser
## Introduction
Parsing through a set of arguments can be an annoying task.  you've got to figure out how
to define them, how to parse them, what starts them, what valid values are, how to validate them,
how to report on errors, and ultimately be able to use the arguments in an efficient way.

I needed a way to read and work with command line parameters.  
I really liked https://github.com/commandlineparser/commandline, but I wanted to do a bit more with it.
So I started with some of the concepts there and started exploring


## Features
- Parses the command line arguments to a class instance
- Attribute driven 
  - You can set attributes for each class property detailing how it will be used
  - Attributes are simple.  no complex names, no stacks of parameters.  
  - Just set the attributes you need.  the component will try to infer the rest
- Fluent interface
  - Parser can use to direct the workflow
  - Definition can use to define the arguments

## Environment
This is developed in C#, and is primarily targetted at the .Net Core part of the universe.  
I'm still working at making this more useful in a Linux as well as the Windows environment, 
so your mileage may vary a bit.

## How to Use
I'm still working on this, but in the future you'll be able to get to it on NuGet.Org

1. Include a reference to the project
2. Create a class to hold your arguments.
3. Set the attributes you need
4. call the parser
4. Enjoy the fruits of your (slightly less dimished) Labors.

## future direction

- [ ] Fluent interface of definitions
- [ ] More Attribute Support
  - [ ] Class level attributes
  - [ ] Validators at the class and property level
  - [ ] Custom attributes
- [ ] More expansive property support on the argument class
- [ ] Action support
- [ ] Better help definition
  - [ ] More documentation
  - [ ] A Tutorial

## License
Lets go with a vanilla MIT license.  Use it, play with it, heck, make it better if you're so inclined.

If you feel you've got some use out of this, and want to show some appreciation, buy me a [Coffee](https://www.buymeacoffee.com/whispersteppe) :smiley:

