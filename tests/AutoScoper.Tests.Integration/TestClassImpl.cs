﻿using Microsoft.Extensions.DependencyInjection;

namespace AutoScoper.Tests.Integration;

public class TestClassImpl : ITestInterface, ITestInterface2, ITestInterface3
{
    public int GetInt(int a, string b)
    {
        return a + int.Parse(b);
    }

    public Task<int> GetIntAsync(int a, string b)
    {
        return Task.FromResult(a + int.Parse(b));
    }

    public Task GetTAsync<T>(int a, string b)
    {
        return Task.CompletedTask;
    }
}
