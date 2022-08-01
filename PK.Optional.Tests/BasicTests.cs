using System;
using Xunit;

namespace PK.Optional.Tests;

public class BasicTests
{
	[Fact]
	public void HasSomeValue()
	{
		var option = new Option<int>(5);
		
		Assert.True(option.HasValue);
		Assert.Equal(5, option.Value);
	}

	[Fact]
	public void HasNoValue()
	{
		var option = new Option<int>();

		Assert.False(option.HasValue);
		Assert.Throws<OptionValueMissingException>(() => option.Value);
	}

	[Fact]
	public void ImplicitConvertTo()
	{
		Option<int> option = 5;

		Assert.True(option.HasValue);
		Assert.Equal(5, option.Value);
	}

	[Fact]
	public void ImplicitConvertFrom()
	{
		var option = new Option<int>(5);
		int val = option;

		Assert.Equal(5, val);
	}

	[Fact]
	public void ComparableEquals()
	{
		var option1 = new Option<int>(5);
		var option2 = new Option<int>(5);

		Assert.Equal(option1, option2);
		Assert.Equal(0, option1.CompareTo(option2));
		Assert.True(option1 == option2);
	}

	[Fact]
	public void ComparableNoneEquals()
	{
		var option1 = new Option<int>();
		var option2 = new Option<int>();

		Assert.Equal(option1, option2);
		Assert.Equal(0, option1.CompareTo(option2));
		Assert.True(option1 == option2);
		Assert.True(option1 >= option2);
		Assert.True(option1 <= option2);
	}

	[Fact]
	public void ComparableNotEquals()
	{
		var option1 = new Option<int>(5);
		var option2 = new Option<int>(7);

		Assert.Equal(-1, option1.CompareTo(option2));
		Assert.Equal(1, option2.CompareTo(option1));
		Assert.False(option1 == option2);
		Assert.True(option1 != option2);
		Assert.True(option1 < option2);
		Assert.True(option1 <= option2);
		Assert.True(option2 > option1);
		Assert.True(option2 >= option1);
	}

	[Fact]
	public void ComparableNoneNotEquals()
	{
		var option1 = new Option<int>();
		var option2 = new Option<int>(5);

		Assert.Equal(-1, option1.CompareTo(option2));
		Assert.Equal(1, option2.CompareTo(option1));

		Assert.False(option1 == option2);
		Assert.True(option1 != option2);
		Assert.True(option1 < option2);
		Assert.True(option1 <= option2);
		Assert.True(option2 > option1);
		Assert.True(option2 >= option1);
	
	}	
}
