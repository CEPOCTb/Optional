using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PK.Optional.Newtonsoft.Json;
using Xunit;

namespace PK.Optional.Tests;

public class NewtonsoftSerializtionTests
{
	[Fact]
	public void OptionalWithNoValueSerialization()
	{
		var testObj = new { A = 1, B = new Optional<string>() };

		var result = JsonConvert.SerializeObject(testObj, new JsonSerializerSettings()
		{
			ContractResolver = new OptionalContractResolver(new DefaultContractResolver())
		});

		Assert.Equal(@"{""A"":1}", result);
	}

	[Fact]
	public void OptionalWithValueSerialization()
	{
		var testObj = new { A = 1, B = new Optional<string>("Test") };

		var result = JsonConvert.SerializeObject(testObj, new JsonSerializerSettings()
		{
			ContractResolver = new OptionalContractResolver(new DefaultContractResolver())
		});

		Assert.Equal(@"{""A"":1,""B"":""Test""}", result);
	}

	[Fact]
	public void OptionalWithNoValueDeserialization()
	{
		var testObj = new { A = 2, B = new Optional<string>("Test1") };

		var result = JsonConvert.DeserializeAnonymousType(@"{""A"":1}", testObj, new JsonSerializerSettings()
		{
			ContractResolver = new OptionalContractResolver(new DefaultContractResolver())
		});

		Assert.Equal(1, result.A);
		Assert.False(result.B.HasValue);
		Assert.Throws<OptionalValueMissingException>(() => result.B.Value == "Test");
	}

	[Fact]
	public void OptionalWithValueDeserialization()
	{
		var testObj = new { A = 2, B = new Optional<string>("Test1") };

		var result = JsonConvert.DeserializeAnonymousType(@"{""A"":1,""B"":""Test""}", testObj, new JsonSerializerSettings()
		{
			ContractResolver = new OptionalContractResolver(new DefaultContractResolver())
		});

		Assert.Equal(1, result.A);
		Assert.True(result.B.HasValue);
		Assert.Equal("Test", result.B.Value);
	}

}