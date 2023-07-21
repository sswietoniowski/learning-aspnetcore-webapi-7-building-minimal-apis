# Learning ASP.NET Core - WebAPI (.NET 7) Building Minimal APIs

This repository contains examples showing how to building a minimal API (WebAPI 7).

Based on this course [Building ASP.NET Core 7 Minimal APIs](https://app.pluralsight.com/library/courses/asp-dot-net-core-7-building-minimal-apis/table-of-contents).

Original course materials can be found [here](https://app.pluralsight.com/library/courses/asp-dot-net-core-7-building-minimal-apis/exercise-files) and [here](https://github.com/KevinDockx/BuildingAspNetCore7MinimalAPIs).

## Table of Contents

- [Learning ASP.NET Core - WebAPI (.NET 7) Building Minimal APIs](#learning-aspnet-core---webapi-net-7-building-minimal-apis)
  - [Table of Contents](#table-of-contents)
  - [Setup](#setup)
  - [Introduction to ASP.NET Core Minimal APIs](#introduction-to-aspnet-core-minimal-apis)
  - [Learning About Core Concepts and Reading Resources](#learning-about-core-concepts-and-reading-resources)
    - [Dependency Injection in Minimal APIs](#dependency-injection-in-minimal-apis)
    - [Learning About Routing](#learning-about-routing)
    - [Working with Routing Templates](#working-with-routing-templates)
    - [Why You Shouldn’t Expose the Entity Model](#why-you-shouldnt-expose-the-entity-model)
    - [Adding the DTO Model and Using AutoMapper](#adding-the-dto-model-and-using-automapper)
    - [Parameter Binding](#parameter-binding)
    - [Modelling Common API Functionality](#modelling-common-api-functionality)
    - [Status Codes and Creating Responses](#status-codes-and-creating-responses)
    - [Creating Correct API Responses](#creating-correct-api-responses)
  - [Manipulating Resources](#manipulating-resources)
    - [Routing, Revisited](#routing-revisited)
    - [Creating a Resource](#creating-a-resource)
    - [Generating Links](#generating-links)
    - [Updating a Resource](#updating-a-resource)
    - [Deleting a Resource](#deleting-a-resource)
    - [Grouping Resources](#grouping-resources)
    - [Content Negotiation in Minimal APIs](#content-negotiation-in-minimal-apis)
    - [Validation in Minimal APIs](#validation-in-minimal-apis)
  - [Structuring Your Minimal API](#structuring-your-minimal-api)
    - [Options for Structuring Minimal APIs](#options-for-structuring-minimal-apis)
    - [Extending IEndpointRouteBuilder to Structure Your Minimal API](#extending-iendpointroutebuilder-to-structure-your-minimal-api)
  - [Handling Exceptions and Logging](#handling-exceptions-and-logging)
    - [Handling Exceptions in Minimal APIs](#handling-exceptions-in-minimal-apis)
    - [Using the Developer Exception Page Middleware](#using-the-developer-exception-page-middleware)
    - [Using the Exception Handler Middleware](#using-the-exception-handler-middleware)
    - [Improving Error Responses with Problem Details](#improving-error-responses-with-problem-details)
    - [Logging in Minimal APIs](#logging-in-minimal-apis)
  - [Implementing Business Logic with Endpoint Filters](#implementing-business-logic-with-endpoint-filters)
    - [Filters for Minimal APIs](#filters-for-minimal-apis)
    - [Creating an Endpoint Filter](#creating-an-endpoint-filter)
    - [Making the Endpoint Filter Reusable](#making-the-endpoint-filter-reusable)
    - [Chaining Endpoint Filters and Applying Them to a Group](#chaining-endpoint-filters-and-applying-them-to-a-group)
    - [Applying Business Logic Depending on the Response](#applying-business-logic-depending-on-the-response)
    - [Handling Request Validation](#handling-request-validation)
  - [Securing Your Minimal API](#securing-your-minimal-api)
    - [High-level API Security Overview](#high-level-api-security-overview)
    - [Token-based Security for Minimal APIs](#token-based-security-for-minimal-apis)
    - [Requiring a Bearer Token](#requiring-a-bearer-token)
    - [Generating a Token](#generating-a-token)
    - [Generating a Token with dotnet-user-jwts](#generating-a-token-with-dotnet-user-jwts)
    - [Creating and Applying an Authorization Policy](#creating-and-applying-an-authorization-policy)
  - [Documenting Your Minimal API](#documenting-your-minimal-api)
    - [A Few Words on Swagger / OpenAPI](#a-few-words-on-swagger--openapi)
    - [Adding Support for OpenAPI with Swashbuckle](#adding-support-for-openapi-with-swashbuckle)
    - [Adding Descriptions and Summaries](#adding-descriptions-and-summaries)
    - [Describing Response Types and Status Codes](#describing-response-types-and-status-codes)
    - [Describing Request Types](#describing-request-types)
    - [Gaining Full OpenApiOperation Control](#gaining-full-openapioperation-control)
    - [Describing API Security in Swagger](#describing-api-security-in-swagger)
  - [Summary](#summary)

## Setup

To run API:

```cmd
cd .\contacts\backend\api
dotnet restore
dotnet ef database update
dotnet build
dotnet watch run
```

## Introduction to ASP.NET Core Minimal APIs

To create a new minimal API project, just choose the "Web API" template in Visual Studio or use the `dotnet new webapi` command like so (just an example):

```cmd
dotnet new webapi --framework net7.0 --language C# --no-https false --auth None --use-minimal-apis true --use-program-main false --output "." --force
```

To test your project you can use:

- :+1: [Postman](https://www.postman.com/) - especially useful for testing REST APIs as presented [here](https://youtu.be/zp5Jh2FIpF0),
- [Insomnia](https://insomnia.rest/),
- [curl](https://curl.se/),
- :+1: [REST Client](https://marketplace.visualstudio.com/items?itemName=humao.rest-client) - my personal favorite as I am a big fan of Visual Studio Code,
- [RESTer](https://addons.mozilla.org/en-US/firefox/addon/rester/) (Firefox Add-on),
- ... and many more.

If you are using Visual Studio or JetBrains Rider you can use the built-in tools. Latest versions of Visual Studio have built-in support for testing REST APIs, more on that [here](https://youtu.be/ud0wx5mgniI).

I'd like to concentrate on the new/different features of the minimal APIs in .NET 7, so I started with an existing code for my domain classes, DTOs, database contexts, etc. What I'll be using as my example will be a simple API for managing contacts.

As this project is using .NET 7 and C# 11, I've decided to try out new language features provided by the latest version of C#. One of them is `required` keyword for properties (+ `[SetsRequiredMembers]` attribute), more on that [here](https://youtu.be/_hQPSOocXs0). I also decided to refactor DTOs to use `record` keyword, more on that [here](https://youtu.be/9Byvwa9yF-I).

## Learning About Core Concepts and Reading Resources

Basic information about core concepts.

### Dependency Injection in Minimal APIs

### Learning About Routing

### Working with Routing Templates

### Why You Shouldn’t Expose the Entity Model

### Adding the DTO Model and Using AutoMapper

### Parameter Binding

### Modelling Common API Functionality

### Status Codes and Creating Responses

### Creating Correct API Responses

## Manipulating Resources

### Routing, Revisited

### Creating a Resource

### Generating Links

### Updating a Resource

### Deleting a Resource

### Grouping Resources

### Content Negotiation in Minimal APIs

### Validation in Minimal APIs

## Structuring Your Minimal API

### Options for Structuring Minimal APIs

### Extending IEndpointRouteBuilder to Structure Your Minimal API

## Handling Exceptions and Logging

### Handling Exceptions in Minimal APIs

### Using the Developer Exception Page Middleware

### Using the Exception Handler Middleware

### Improving Error Responses with Problem Details

### Logging in Minimal APIs

## Implementing Business Logic with Endpoint Filters

### Filters for Minimal APIs

### Creating an Endpoint Filter

### Making the Endpoint Filter Reusable

### Chaining Endpoint Filters and Applying Them to a Group

### Applying Business Logic Depending on the Response

### Handling Request Validation

## Securing Your Minimal API

### High-level API Security Overview

### Token-based Security for Minimal APIs

### Requiring a Bearer Token

### Generating a Token

### Generating a Token with dotnet-user-jwts

### Creating and Applying an Authorization Policy

## Documenting Your Minimal API

### A Few Words on Swagger / OpenAPI

### Adding Support for OpenAPI with Swashbuckle

### Adding Descriptions and Summaries

### Describing Response Types and Status Codes

### Describing Request Types

### Gaining Full OpenApiOperation Control

### Describing API Security in Swagger

## Summary
