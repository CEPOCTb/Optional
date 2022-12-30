using System;
using Xunit;

namespace PK.Optional.Tests;

public class StaticTests
{
	[Fact]
	public void Create()
	{
		var option = Optional.FromValue(5);

		Assert.True(option.HasValue);
		Assert.Equal(5, option.Value);
	}
	
	[Fact]
	public void CreateNone()
	{
		var option = Optional.Empty<int>();

		Assert.False(option.HasValue);
		Assert.Throws<OptionalValueMissingException>(() => option.Value);
	}

	[Fact]
	public void CreateExplicitNone()
	{
		var option = Optional.Empty<int>();

		Assert.False(option.HasValue);
		Assert.Throws<OptionalValueMissingException>(() => option.Value);
	}

	[Fact]
	public void IsOption()
	{
		var option = Optional.Empty<int>();
		Assert.True(option.IsOptional());

		Assert.False(5.IsOptional());
	}

	[Fact]
	public void HasValue()
	{
		var option = Optional.FromValue(5);
		Assert.True(Optional.HasValue(option));

		option = Optional.Empty<int>();
		Assert.False(Optional.HasValue(option));
	}

	[Fact]
	public void HasValueThrows()
	{
		Assert.Throws<ObjectNotAnOptionalException>(() => Optional.HasValue(5));
	}

	[Fact]
	public void GetValue()
	{
		var option = Optional.FromValue(5);
		Assert.Equal(5, Optional.GetValue(option));
	}

	[Fact]
	public void GetValueThrowsNonOption()
	{
		Assert.Throws<ObjectNotAnOptionalException>(() => Optional.GetValue(5));
	}

	[Fact]
	public void GetValueThrowsNone()
	{
		var option = Optional.Empty<int>();
		Assert.Throws<OptionalValueMissingException>(() => Optional.GetValue(option));
	}
	
}
