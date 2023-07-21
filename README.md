# Learning ASP.NET Core - WebAPI (.NET 7) Building Minimal APIs

This repository contains examples showing how to building a minimal API (WebAPI 7).

Based on this course [Building ASP.NET Core 7 Minimal APIs](https://app.pluralsight.com/library/courses/asp-dot-net-core-7-building-minimal-apis/table-of-contents).

Original course materials can be found [here](https://app.pluralsight.com/library/courses/asp-dot-net-core-7-building-minimal-apis/exercise-files) and [here](https://github.com/KevinDockx/BuildingAspNetCore7MinimalAPIs).

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

### Building APIs with ASP.NET Core

### Creating and Inspecting an ASP.NET Core Minimal API Project

### Using Postman

### Adding the Data Layer

## Learning About Core Concepts and Reading Resources

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
