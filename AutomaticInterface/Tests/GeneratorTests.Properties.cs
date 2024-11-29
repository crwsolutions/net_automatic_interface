﻿using FluentAssertions;
using Xunit;

namespace Tests;

public partial class GeneratorTests
{
    [Fact]
    public void OmitsPrivateSetPropertyInterface()
    {
        const string code = """

            using AutomaticInterface;

            namespace AutomaticInterfaceExample
            {

                [GenerateAutomaticInterface]
                class DemoClass
                {
                    /// <inheritdoc />
                    public string Hello { get; private set; }
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
                    /// <inheritdoc />
                    string Hello { get; }
                    
                }
            }

            """;
        GenerateCode(code).Should().Be(expected);
    }

    [Fact]
    public void CopiesDocumentationOfPropertyToInterface()
    {
        const string code = """

            using AutomaticInterface;

            namespace AutomaticInterfaceExample
            {

                [GenerateAutomaticInterface]
                class DemoClass
                {
                    /// <summary>
                    /// Bla bla
                    /// </summary>
                    public string Hello { get; private set; }
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
                    /// <inheritdoc />
                    string Hello { get; }
                    
                }
            }

            """;
        GenerateCode(code).Should().Be(expected);
    }

    [Fact]
    public void NullableProperty()
    {
        const string code = """

            using AutomaticInterface;
            using System;

            namespace AutomaticInterfaceExample
            {
                    /// <summary>
                    /// Bla bla
                    /// </summary>
                [GenerateAutomaticInterface]
                class DemoClass
                {

                    /// <summary>
                    /// Bla bla
                    /// </summary>
                    public string? NullableProperty { get; set; }
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

            #nullable enable
            namespace AutomaticInterfaceExample
            {
                /// <summary>
                /// Bla bla
                /// </summary>
                [global::System.CodeDom.Compiler.GeneratedCode("AutomaticInterface", "")]
                public partial interface IDemoClass
                {
                    /// <inheritdoc />
                    string? NullableProperty { get; set; }
                    
                }
            }
            #nullable restore

            """;
        GenerateCode(code).Should().Be(expected);
    }

    [Fact]
    public void NullableProperty2()
    {
        const string code = """

            using AutomaticInterface;
            using System;
            using System.Threading.Tasks;

            namespace AutomaticInterfaceExample
            {
                    /// <summary>
                    /// Bla bla
                    /// </summary>
                [GenerateAutomaticInterface]
                class DemoClass
                {

                    /// <summary>
                    /// Bla bla
                    /// </summary>
                    public Task<string?> NullableProperty { get; set; }
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

            #nullable enable
            namespace AutomaticInterfaceExample
            {
                /// <summary>
                /// Bla bla
                /// </summary>
                [global::System.CodeDom.Compiler.GeneratedCode("AutomaticInterface", "")]
                public partial interface IDemoClass
                {
                    /// <inheritdoc />
                    global::System.Threading.Tasks.Task<string?> NullableProperty { get; set; }
                    
                }
            }
            #nullable restore

            """;
        GenerateCode(code).Should().Be(expected);
    }

    [Fact]
    public void WorksWithNewKeyword()
    {
        const string code = """

            using AutomaticInterface;
            using System.Threading.Tasks;

            namespace AutomaticInterfaceExample;

            public abstract class FirstClass
            {
                public int AProperty { get; set; }
            }

            [GenerateAutomaticInterface]
            public partial class SecondClass : FirstClass
            {
                public new int AProperty { get; set; }
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
                public partial interface ISecondClass
                {
                    /// <inheritdoc />
                    int AProperty { get; set; }
                    
                }
            }

            """;
        GenerateCode(code).Should().Be(expected);
    }

    [Fact]
    public void WorksWithPropertyShadowing()
    {
        const string code = """
            using AutomaticInterface;
            using System;

            namespace AutomaticInterfaceExample;

            public class BaseClass
            {
                public string SomeProperty { get; set; }
            }

            [GenerateAutomaticInterface]
            public class DemoClass : BaseClass
            {
                public new string SomeProperty { get; set; }
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
                    /// <inheritdoc />
                    string SomeProperty { get; set; }
                    
                }
            }

            """;
        GenerateCode(code).Should().Be(expected);
    }

    [Fact]
    public void WorksWithPropertyOverrides()
    {
        const string code = """
            using AutomaticInterface;
            using System;

            namespace AutomaticInterfaceExample;

            public class BaseClass
            {
                public virtual string SomeProperty { get; set; }
            }

            [GenerateAutomaticInterface]
            public class DemoClass : BaseClass
            {
                public override string SomeProperty { get; set; }
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
                    /// <inheritdoc />
                    string SomeProperty { get; set; }
                    
                }
            }

            """;
        GenerateCode(code).Should().Be(expected);
    }

    [Fact]
    public void GeneratesStringPropertyInterface()
    {
        const string code = """

            using AutomaticInterface;

            namespace AutomaticInterfaceExample
            {

                [GenerateAutomaticInterface]
                class DemoClass
                {
                    public string Hello { get; set; }
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
                    /// <inheritdoc />
                    string Hello { get; set; }
                    
                }
            }

            """;
        GenerateCode(code).Should().Be(expected);
    }

    [Fact]
    public void GeneratesStringPropertySetOnlyInterface()
    {
        const string code = """

            using AutomaticInterface;

            namespace AutomaticInterfaceExample
            {

                [GenerateAutomaticInterface]
                class DemoClass
                {
                    private string x;
                    public string Hello { set => x = value; }
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
                    /// <inheritdoc />
                    string Hello { set; }
                    
                }
            }

            """;
        GenerateCode(code).Should().Be(expected);
    }

    [Fact]
    public void GeneratesStringPropertyGetOnlyInterface()
    {
        const string code = """

            using AutomaticInterface;

            namespace AutomaticInterfaceExample
            {

                [GenerateAutomaticInterface]
                class DemoClass
                {
                    private string x;
                    public string Hello { get; }
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
                    /// <inheritdoc />
                    string Hello { get; }
                    
                }
            }

            """;
        GenerateCode(code).Should().Be(expected);
    }

    [Fact]
    public void GeneratesInitPropertyInterface()
    {
        const string code = """

            using AutomaticInterface;

            namespace AutomaticInterfaceExample
            {

                [GenerateAutomaticInterface]
                class DemoClass
                {
                    public string Hello { get; init; }
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
                    /// <inheritdoc />
                    string Hello { get; init; }
                    
                }
            }

            """;
        GenerateCode(code).Should().Be(expected);
    }
}