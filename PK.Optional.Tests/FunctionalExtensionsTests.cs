using System;
using System.Diagnostics.CodeAnalysis;
using PK.Optional.FunctionalExtensions;
using Xunit;

namespace PK.Optional.Tests;

public class FunctionalExtensionsTests
{
	[Fact]
	public void CreateSome()
	{
		var option = 5.Some();

		Assert.True(option.HasValue);
		Assert.Equal(5, option.Value);
	}

	[Fact]
	public void CreateNone()
	{
		var option = 5.None();

		Assert.False(option.HasValue);
		Assert.Throws<OptionalValueMissingException>(() => option.Value);
	}

	[Fact]
	public void CreateNone2()
	{
		var option = Functional.None<int>();

		Assert.False(option.HasValue);
		Assert.Throws<OptionalValueMissingException>(() => option.Value);
	}

	[Fact]
	public void ValueOrDefaultValue()
	{
		var option = 5.Some();
		Assert.Equal(5, option.ValueOrDefault());
	}

	[Fact]
	public void ValueOrDefaultDefault()
	{
		var option = 5.None();
		Assert.Equal(0, option.ValueOrDefault());
	}

	[Fact]
	public void ValueOr()
	{
		var option = 5.Some();
		Assert.Equal(5, option.ValueOr(7));
	}

	[Fact]
	public void NoneValueOr()
	{
		var option = 5.None();
		Assert.Equal(7, option.ValueOr(7));
	}

	[Fact]
	public void ValueOrFunc()
	{
		var option = 5.Some();
		Assert.Equal(5, option.ValueOr(() => 7));
	}

	[Fact]
	public void NoneValueOrFunc()
	{
		var option = 5.None();
		Assert.Equal(7, option.ValueOr(() => 7));
	}

	[Fact]
	public void NullValueOrFunc()
	{
		var option = 5.None();
		Assert.Throws<ArgumentNullException>(() => option.ValueOr(null));
	}

	[Fact]
	public void Or()
	{
		var option = 5.Some();
		var orOption = option.Or(7);
		
		Assert.True(orOption.HasValue);
		Assert.Equal(5, orOption.Value);
	}

	[Fact]
	public void NoneOr()
	{
		var option = 5.None();
		var orOption = option.Or(7);
		
		Assert.True(orOption.HasValue);
		Assert.Equal(7, orOption.Value);
	}


	[Fact]
	public void OrFunc()
	{
		var option = 5.Some();
		var orOption = option.Or(() => 7);

		Assert.True(orOption.HasValue);
		Assert.Equal(5, orOption.Value);
	}

	[Fact]
	public void NoneOrFunc()
	{
		var option = 5.None();
		var orOption = option.Or(() => 7);

		Assert.True(orOption.HasValue);
		Assert.Equal(7, orOption.Value);
	}

	[Fact]
	public void NullOrFunc()
	{
		var option = 5.None();
		Assert.Throws<ArgumentNullException>(() => option.Or(null));
	}

	[Fact]
	public void Else()
	{
		var option = 5.Some();
		var elseOption = option.Else(7.Some());

		Assert.True(elseOption.HasValue);
		Assert.Equal(5, elseOption.Value);
	}

	[Fact]
	public void NoneElse()
	{
		var option = 5.None();
		var elseOption = option.Else(7.Some());

		Assert.True(elseOption.HasValue);
		Assert.Equal(7, elseOption.Value);
	}


	[Fact]
	public void ElseFunc()
	{
		var option = 5.Some();
		var elseOption = option.Else(() => 7.Some());

		Assert.True(elseOption.HasValue);
		Assert.Equal(5, elseOption.Value);
	}

	[Fact]
	public void NoneElseFunc()
	{
		var option = 5.None();
		var elseOption = option.Else(() => 7.Some());

		Assert.True(elseOption.HasValue);
		Assert.Equal(7, elseOption.Value);
	}

	[Fact]
	public void NullElseFunc()
	{
		var option = 5.None();
		Assert.Throws<ArgumentNullException>(() => option.Else(null));
	}


	[Fact]
	public void MatchWithSomeFunc()
	{
		var option = 5.Some();
		var match = option.Match(i => i + 1, () => 1);
		
		Assert.Equal(6, match);
	}

	[Fact]
	public void MatchWithNoneFunc()
	{
		var option = 5.None();
		var match = option.Match(i => i + 1, () => 1);

		Assert.Equal(1, match);
	}

	[Fact]
	public void MatchWithSomeFuncNull()
	{
		var option = 5.Some();
		Assert.Throws<ArgumentNullException>(() => option.Match(null, () => 1));
		Assert.Throws<ArgumentNullException>(() => option.Match(i => i + 1, null));
	}

	[Fact]
	public void MatchWithNoneFuncNull()
	{
		var option = 5.None();
		Assert.Throws<ArgumentNullException>(() => option.Match(null, () => 1));
		Assert.Throws<ArgumentNullException>(() => option.Match(i => i + 1, null));
	}

	[Fact]
	public void MatchWithSomeAction()
	{
		var option = 5.Some();
		var someCalled = 0;
		var noneCalled = 0;
		
		option.Match(i =>
		{
			Assert.Equal(5, i);
			someCalled++;
		}, () =>
		{
			noneCalled++;
		});

		Assert.Equal(1, someCalled);
		Assert.Equal(0, noneCalled);
	}

	[Fact]
	public void MatchWithNoneAction()
	{
		var option = 5.None();
		var someCalled = 0;
		var noneCalled = 0;

		option.Match(
			i =>
			{
				someCalled++;
			},
			() =>
			{
				noneCalled++;
			});

		Assert.Equal(0, someCalled);
		Assert.Equal(1, noneCalled);
	}

	[Fact]
	public void MatchWithSomeActionNull()
	{
		var option = 5.Some();
		Assert.Throws<ArgumentNullException>(() => option.Match(null, () => {}));
		Assert.Throws<ArgumentNullException>(() => option.Match(i => {}, null));
	}

	[Fact]
	public void MatchWithNoneActionNull()
	{
		var option = 5.None();
		Assert.Throws<ArgumentNullException>(() => option.Match(null, () => {}));
		Assert.Throws<ArgumentNullException>(() => option.Match(i => {}, null));
	}

	[Fact]
	public void MatchSomeWithSome()
	{
		var option = 5.Some();
		var someCalled = 0;

		option.MatchSome(
			i =>
			{
				Assert.Equal(5, i);
				someCalled++;
			});

		Assert.Equal(1, someCalled);
	}

	[Fact]
	public void MatchSomeWithNone()
	{
		var option = 5.None();
		var someCalled = 0;

		option.MatchSome(
			i =>
			{
				Assert.Equal(5, i);
				someCalled++;
			});

		Assert.Equal(0, someCalled);
	}

	[Fact]
	public void MatchSomeActionNull()
	{
		var option = 5.Some();
		Assert.Throws<ArgumentNullException>(() => option.MatchSome(null));
		
		option = 5.None();
		Assert.Throws<ArgumentNullException>(() => option.MatchSome(null));
	}

	[Fact]
	public void MatchNoneWithNone()
	{
		var option = 5.None();
		var someCalled = 0;

		option.MatchNone(
			() =>
			{
				someCalled++;
			});

		Assert.Equal(1, someCalled);
	}

	[Fact]
	public void MatchNoneWithSome()
	{
		var option = 5.Some();
		var someCalled = 0;

		option.MatchNone(
			() =>
			{
				someCalled++;
			});

		Assert.Equal(0, someCalled);
	}

	[Fact]
	public void MatchNoneActionNull()
	{
		var option = 5.Some();
		Assert.Throws<ArgumentNullException>(() => option.MatchNone(null));

		option = 5.None();
		Assert.Throws<ArgumentNullException>(() => option.MatchNone(null));
	}

	[Fact]
	public void MapSome()
	{
		var option = 5.Some();
		var mapped = option.Map(o => o.ToString());
		Assert.True(mapped.HasValue);
		Assert.Equal("5", mapped);
	}

	[Fact]
	public void MapNone()
	{
		var option = 5.None();
		var mapped = option.Map(o => o.ToString());
		Assert.False(mapped.HasValue);
		Assert.Throws<OptionalValueMissingException>(() => mapped.Value);
	}

	[Fact]
	public void MapNullFunc()
	{
		var option = 5.Some();
		Assert.Throws<ArgumentNullException>(() => option.Map<int, string>(null));

		option = 5.None();
		var mapped = option.Map(o => o.ToString());
		Assert.False(mapped.HasValue);
		Assert.Throws<OptionalValueMissingException>(() => mapped.Value);
	}

	[Fact]
	public void FlatMapSome()
	{
		var option = 5.Some();
		var mapped = option.FlatMap(o => o.ToString().Some());
		Assert.True(mapped.HasValue);
		Assert.Equal("5", mapped);
	}

	[Fact]
	public void FlatMapNone()
	{
		var option = 5.None();
		var mapped = option.FlatMap(o => o.ToString().Some());
		Assert.False(mapped.HasValue);
		Assert.Throws<OptionalValueMissingException>(() => mapped.Value);
	}

	[Fact]
	public void FlatMapNullFunc()
	{
		var option = 5.Some();
		Assert.Throws<ArgumentNullException>(() => option.FlatMap<int, string>(null));

		option = 5.None();
		var mapped = option.FlatMap(o => o.ToString().Some());
		Assert.False(mapped.HasValue);
		Assert.Throws<OptionalValueMissingException>(() => mapped.Value);
	}

	[Fact]
	public void FilterTest()
	{
		var option = 5.Some();
		var filtered = option.Filter(true);
		Assert.True(filtered.HasValue);
		Assert.Equal(5, filtered.Value);

		filtered = option.Filter(false);
		Assert.False(filtered.HasValue);
		Assert.Throws<OptionalValueMissingException>(() => filtered.Value);

		option = 5.None();
		filtered = option.Filter(true);
		Assert.False(filtered.HasValue);
		Assert.Throws<OptionalValueMissingException>(() => filtered.Value);

		filtered = option.Filter(false);
		Assert.False(filtered.HasValue);
		Assert.Throws<OptionalValueMissingException>(() => filtered.Value);
	}

	[Fact]
	[SuppressMessage("ReSharper", "AccessToModifiedClosure")]
	public void FilterTestFunc()
	{
		var option = 5.Some();
		int called = 0; 
		var filtered = option.Filter(
			val =>
			{
				Assert.Equal(5, val);
				called++;
				return true;
			});
		Assert.True(filtered.HasValue);
		Assert.Equal(5, filtered.Value);
		Assert.Equal(1, called);

		called = 0;
		filtered = option.Filter(
			val =>
			{
				Assert.Equal(5, val);
				called++;
				return false;
			});
		Assert.False(filtered.HasValue);
		Assert.Throws<OptionalValueMissingException>(() => filtered.Value);
		Assert.Equal(1, called);

		option = 5.None();
		called = 0;
		filtered = option.Filter(
			val =>
			{
				Assert.Equal(5, val);
				called++;
				return true;
			});
		Assert.False(filtered.HasValue);
		Assert.Throws<OptionalValueMissingException>(() => filtered.Value);
		Assert.Equal(0, called);

		called = 0;
		filtered = option.Filter(
			val =>
			{
				Assert.Equal(5, val);
				called++;
				return false;
			});
		Assert.False(filtered.HasValue);
		Assert.Throws<OptionalValueMissingException>(() => filtered.Value);
		Assert.Equal(0, called);
	}

	[Fact]
	public void NotNull()
	{
		var option = "5".Some();
		var notnull = option.NotNull();
		Assert.True(notnull.HasValue);
		Assert.Equal("5", notnull.Value);

		option = "5".None();
		notnull = option.NotNull();
		Assert.False(notnull.HasValue);
		Assert.Throws<OptionalValueMissingException>(() => notnull.Value);

		option = ((string)null).Some();
		notnull = option.NotNull();
		Assert.False(notnull.HasValue);
		Assert.Throws<OptionalValueMissingException>(() => notnull.Value);

		option = ((string)null).None();
		notnull = option.NotNull();
		Assert.False(notnull.HasValue);
		Assert.Throws<OptionalValueMissingException>(() => notnull.Value);
	}
}
