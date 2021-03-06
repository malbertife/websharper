// $begin{copyright}
//
// This file is part of WebSharper
//
// Copyright (c) 2008-2014 IntelliFactory
//
// Licensed under the Apache License, Version 2.0 (the "License"); you
// may not use this file except in compliance with the License.  You may
// obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or
// implied.  See the License for the specific language governing
// permissions and limitations under the License.
//
// $end{copyright}

/// Defines and manipulates compiler metadata associated with assemblies.
module internal IntelliFactory.WebSharper.Compiler.Metadata

module C = IntelliFactory.JavaScript.Core
module M = IntelliFactory.WebSharper.Core.Metadata
module P = IntelliFactory.JavaScript.Packager
module Q = IntelliFactory.WebSharper.Core.Quotations
module R = IntelliFactory.WebSharper.Core.Reflection
module Re = IntelliFactory.WebSharper.Core.Remoting
module V = IntelliFactory.WebSharper.Compiler.Validator

/// Represents positions in a list.
type Position = int

/// Represents JavaScript record field names.
type Field = string

/// Represents constructor metadata.
type ConstructorKind =
    | BasicConstructor of P.Address
    | InlineConstructor of Inlining.Inline
    | MacroConstructor of R.Type
    | StubConstructor of P.Address

/// Represents data type metadata.
type DataTypeKind =
    | Class of P.Address
    | Exception of P.Address
    | Interface of P.Address
    | Object of list<string * string>
    | Record of P.Address * list<string * string>

/// Represents method metadata.
type MethodKind =
    | BasicInstanceMethod of Name
    | BasicStaticMethod of P.Address
    | InlineMethod of Inlining.Inline
    | MacroMethod of R.Type
    | RemoteMethod of MemberScope * V.RemotingKind * M.MethodHandle

/// Represents property metadata.
type PropertyKind =
    | BasicProperty of option<MethodKind> * option<MethodKind>
    | FieldProperty of int
    | InstanceStubProperty of Name
    | InterfaceProperty of Name
    | StaticStubProperty of P.Address

/// Represents union metadata.
type UnionCaseKind =
    | BasicUnionCase of Position
    | CompiledUnionCase of P.Address * Position
    | ConstantUnionCase of Value

/// Represents a metadata record associated with an assembly.
[<Sealed>]
type T =

    /// Maps a constructor to its metadata.
    member Constructor : R.Constructor -> option<ConstructorKind>

    /// Maps a class or module definition to its metadata.
    member DataType : R.TypeDefinition -> option<DataTypeKind>

    /// Maps a method to its metadata.
    member Method : R.Method -> option<MethodKind>

    /// Maps a property to its metadata.
    member Property : R.Property -> option<PropertyKind>

    /// Maps a union case to its metadata.
    member UnionCase : R.UnionCase -> option<UnionCaseKind>

    /// The empty metadata record.
    static member Empty : T

/// Parses metadata from an reflected assembly.
val Parse : Logger -> Validator.Assembly -> T

/// Takes a union of several metadata records.
val Union : Logger -> seq<T> -> T

/// Writes metadata in binary representation to a stream.
val Serialize : System.IO.Stream -> T -> unit

/// Reads metadata from a binary representation.
val Deserialize : System.IO.Stream -> T

/// Reads from a Mono.Cecil assembly.
val ReadFromCecilAssembly : Mono.Cecil.AssemblyDefinition -> option<T>
