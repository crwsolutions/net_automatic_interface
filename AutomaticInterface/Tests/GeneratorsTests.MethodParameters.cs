﻿using FluentAssertions;
using Xunit;

namespace Tests;

public partial class GeneratorTests
{
    [Fact]
    public void WorksWithMethodOutParameter()
    {
        const string code = """

            using AutomaticInterface;
            using System.Threading.Tasks;

            namespace AutomaticInterfaceExample;
            [GenerateAutomaticInterface]
            public class DemoClass
            {
                public void AMethod(out int someOutParameter)
                {
                    someOutParameter = 1;
                }
            }

            """;

        const string expected = """
            //--------------------------------------------------------------------------------------------------
            // <auto-generated>
            //     This code was generated by a tool.
            //
            //     Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
            // </auto-generated>
            //--------------------------------------------------------------------------------------------------

            namespace AutomaticInterfaceExample
            {
                [global::System.CodeDom.Compiler.GeneratedCode("AutomaticInterface", "")]
                public partial interface IDemoClass
                {
                    /// <inheritdoc cref="AutomaticInterfaceExample.DemoClass.AMethod(out int)" />
                    void AMethod(out int someOutParameter);
                    
                }
            }

            """;
        GenerateCode(code).Should().Be(expected);
    }

    [Fact]
    public void WorksWithMethodInParameter()
    {
        const string code = """

            using AutomaticInterface;
            using System.Threading.Tasks;

            namespace AutomaticInterfaceExample;
            [GenerateAutomaticInterface]
            public class DemoClass
            {
                public void AMethod(in int someOutParameter)
                {
                }
            }

            """;

        const string expected = """
            //--------------------------------------------------------------------------------------------------
            // <auto-generated>
            //     This code was generated by a tool.
            //
            //     Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
            // </auto-generated>
            //--------------------------------------------------------------------------------------------------

            namespace AutomaticInterfaceExample
            {
                [global::System.CodeDom.Compiler.GeneratedCode("AutomaticInterface", "")]
                public partial interface IDemoClass
                {
                    /// <inheritdoc cref="AutomaticInterfaceExample.DemoClass.AMethod(in int)" />
                    void AMethod(in int someOutParameter);
                    
                }
            }

            """;
        GenerateCode(code).Should().Be(expected);
    }

    [Fact]
    public void WorksWithMethodRefParameter()
    {
        const string code = """

            using AutomaticInterface;
            using System.Threading.Tasks;

            namespace AutomaticInterfaceExample;
            [GenerateAutomaticInterface]
            public class DemoClass
            {
                public void AMethod(ref int someOutParameter)
                {
                }
            }

            """;

        const string expected = """
            //--------------------------------------------------------------------------------------------------
            // <auto-generated>
            //     This code was generated by a tool.
            //
            //     Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
            // </auto-generated>
            //--------------------------------------------------------------------------------------------------

            namespace AutomaticInterfaceExample
            {
                [global::System.CodeDom.Compiler.GeneratedCode("AutomaticInterface", "")]
                public partial interface IDemoClass
                {
                    /// <inheritdoc cref="AutomaticInterfaceExample.DemoClass.AMethod(ref int)" />
                    void AMethod(ref int someOutParameter);
                    
                }
            }

            """;
        GenerateCode(code).Should().Be(expected);
    }
}
