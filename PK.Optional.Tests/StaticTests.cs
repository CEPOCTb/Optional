using System;
using Xunit;

namespace PK.Optional.Tests;

public class StaticTests
{
	[Fact]
	public void Create()
	{
		var option = Option.Some(5);

		Assert.True(option.HasValue);
		Assert.Equal(5, option.Value);
	}
	
	[Fact]
	public void CreateNone()
	{
		var option = Option.None<int>();

		Assert.False(option.HasValue);
		Assert.Throws<OptionValueMissingException>(() => option.Value);
	}

	[Fact]
	public void CreateExplicitNone()
	{
		var option = Option.None<int>();

		Assert.False(option.HasValue);
		Assert.Throws<OptionValueMissingException>(() => option.Value);
	}

	[Fact]
	public void IsOption()
	{
		var option = Option.None<int>();
		Assert.True(option.IsOption());

		Assert.False(5.IsOption());
	}

	[Fact]
	public void HasValue()
	{
		var option = Option.Some(5);
		Assert.True(Option.HasValue(option));

		option = Option.None<int>();
		Assert.False(Option.HasValue(option));
	}

	[Fact]
	public void HasValueThrows()
	{
		Assert.Throws<ObjectNotAnOptionException>(() => Option.HasValue(5));
	}

	[Fact]
	public void GetValue()
	{
		var option = Option.Some(5);
		Assert.Equal(5, Option.GetValue(option));
	}

	[Fact]
	public void GetValueThrowsNonOption()
	{
		Assert.Throws<ObjectNotAnOptionException>(() => Option.GetValue(5));
	}

	[Fact]
	public void GetValueThrowsNone()
	{
		var option = Option.None<int>();
		Assert.Throws<OptionValueMissingException>(() => Option.GetValue(option));
	}
	
}
