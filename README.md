# Capgemini.CodeAnalysis

#Code Analyzer - Microsoft .NET Compiler Platform("Roslyn")
##Content
 - [Introduction](#introduction)
 - [Installation](#installation)
 - [Implementation](#implementation)
 - [Deployment](#deployment)
 - [Usage](#usage)
----
## Introduction
A compiler is a program which translates the source code into the executable code.

It also detects syntax and semantic error and it used to be a black-box operation.

However, the Microsoft .NET compiler platform ("Roslyn") has variety of code analysis API which could be extended into the existing compiler to make own custom code analysis program.

----
## Usage

Capgemini IP code analyzer enforces the following rules

 - CommentsAnalyzer - Enforces that excessive amounts of comments are added to variables
- ExplicitAccessModifiersAnalyzer - Enforces that methods, types and variables have explicit modifiers
- FileHierarchyAnalyzer - Enforces that namespaces align with file hierarchy structure
- GodClassAnalyzer - Enforces that no class contains more than 20 methods 
- HardCodeAnalyzer - Enforces that literal strings are not part of the general code but moved into constants. This analyser is currently a todo item
- IfStatementAnalyzer - Enforces that all If statements are guarded by curly braces
- LargeCommentedCodeAnalyzer - Enforces that excessive amounts of comments are added to any part of the code base
- LoopStatementAnalyzer - Enforces that all loop statements are guarded by curly braces
- MethodComplexityAnalyzer - Enforces that no method's cyclomatic complexity is greater than 15
- MonsterMethodAnalyzer - Enforces that no method has more than 80 executable lines of code
- NamingConventionAnalyzer - Enforces that all artefacts within the code base are named according to predefined patterns
- OneTypePerFileAnalyzer - Enforces that only one type is defined within a C# file
- StaticClassAnalyzer - Enforces that static classes are not used unless absolutely required
- XmlCommentsAnalyzer - Identifies public methods which do not have valid comments


----
## Installation
In order to write custom code analyzers, refactoring, and stand-alone .NET applications that utilizes the Roslyn APIs, you need to install the **.NET Compiler Platform SDK.**

Open Visual Studio 2015 -> Select **Tools** > **Extensions and Updates**. In the Extensions and Updates dialog box,select Online on the left, and then in the serach box, tpye 
**.NET Compiler Platform SDK.**

More information can be found [here](https://msdn.microsoft.com/en-us/library/mt162308.aspx)

----
## Implementation
It is critical for the DevOps team to communicate well with other scrum teams to gather the right code analysis logics and implement correctly to maintain the good coding standard.

In order to create custom code analysis program, you have to investigte the syntax of the code which will be analysed. This can be done by using *Syntax visualizer*.

The **Syntax Visualizer** can be enabled via **View** > **Other Windows** > **Syntax visualizer**

#### Syntax Visualizer 

Syntax visualizer allows you to inspect the syntax tree by the .NET platform "Roslyn"

![codeSnippet1.PNG](.attachments/codeSnippet1-89db3d72-085d-4af6-aee8-9db96faac10d.PNG =900x400)
#### Implementing the Analysis Logic

The analysis logic requires two fundamental steps:

- Write a method tha will perform the code analysis over a given syntax node

- Register the action at the  analyzer's startup so that the analyzer can respond to compiler events

#### Method used to register action

**RegisterSyntaxNodeAction** - Register an action that is exectued at the completion of the semantic analysis of a syntax node.

*Example*
```c#
public override void Initialize(AnalysisContext context)
{
    context.RegisterSyntaxNodeAction(MethodName, SyntaxKind.IdentifierName);
}
```

```c#
private void MethodName(SyntaxNodeAnalysisContext context)
{
    var root = context.Node;
    //Code analysis logic implementation

    //Create a diagnostic at the node location
    var diagnostics = Diagnostics.Create(Rule, root.GetLocation(), message);
    context.ReportDiagnostic(diagnostics);
}
```

----
## Deployment
Once the code analysis logics are implemented and tested, the project will produce an analyzer .dll, as well as the

- A Nuget Package(.nupkg file) which will add your assembly as a project-local analyzer that  participates in builds
- A VSIX extension(.vsix file) that will apply your analyzer to all projects and works just in the IDE

#### Applying your Nuget Package

- Create a local Nuget feed
- Copy the .nukpg file into that folder
- Right-click on the project node in Solution Explorer and choose Mange Nuget Packages
- Select the Nuget feed you created on the left 
- Choose your analyzer from the list and click install

Note: Capgemini.CodeAnalysis is currently integrated into a continuous integration pipeline in VSTS which generates associated nugget packages [here](https://capgeminiuk.visualstudio.com/Capgemini%20Reusable%20IP/Capgemini%20Reusable%20IP%20Team/_packaging?feed=CapgeminiIp&_a=feed).

Currently, we have the following code analysers:
- **Capgemini.CodeAnalysis.CoreAnalysers** - Contains analysers that are applicable to any C# code bases
- **Capgemini.CodeAnalysis.XrmAnalysers** - Contains analysers that are designed to enforce rules that are applicable to CRM code bases

These packages can be consumed by adding the [Capgemini NuGet Feed](https://capgeminiuk.pkgs.visualstudio.com/_packaging/CapgeminiIp/nuget/v3/index.json) to the NuGet config of the target C# solution

----
