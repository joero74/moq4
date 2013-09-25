using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;

using Xunit;

namespace Moq.Tests
{
	public class EmptyAsyncDefaultValueProviderFixture
	{
		[Fact]
		public void ProvidesFinishedTask()
		{
			var provider = new EmptyDefaultValueProvider();

			var value = provider.ProvideDefault(typeof(IFoo).GetMethod("MethodAsync"));

			Assert.IsAssignableFrom<Task>(value);

			var task = (Task)value;
			Assert.True(task.IsCompleted);
		}

		[Fact]
		public void ProvidesNullString()
		{
			var provider = new EmptyDefaultValueProvider();

			var value = provider.ProvideDefault(typeof(IFoo).GetMethod("StringValueAsync"));

			Assert.IsType<Task<string>>(value);

			var task = (Task<string>)value;
			Assert.Null(task.Result);
		}

		[Fact]
		public void ProvidesDefaultInt()
		{
			var provider = new EmptyDefaultValueProvider();

			var value = provider.ProvideDefault(typeof(IFoo).GetMethod("IntValueAsync"));

			Assert.IsType<Task<int>>(value);

			var task = (Task<int>)value;
			Assert.Equal(default(int), task.Result);
		}

		[Fact]
		public void ProvidesNullInt()
		{
			var provider = new EmptyDefaultValueProvider();

			var value = provider.ProvideDefault(typeof(IFoo).GetMethod("NullableIntValueAsync"));

			Assert.IsType<Task<int?>>(value);

			var task = (Task<int?>)value;
			Assert.Null(task.Result);
		}

		[Fact]
		public void ProvidesDefaultBool()
		{
			var provider = new EmptyDefaultValueProvider();

			var value = provider.ProvideDefault(typeof(IFoo).GetMethod("BoolValueAsync"));

			Assert.IsType<Task<bool>>(value);

			var task = (Task<bool>)value;
			Assert.Equal(default(bool), task.Result);
		}

		[Fact]
		public void ProvidesDefaultEnum()
		{
			var provider = new EmptyDefaultValueProvider();

			var value = provider.ProvideDefault(typeof(IFoo).GetMethod("PlatformAsync"));

			Assert.IsType<Task<PlatformID>>(value);

			var task = (Task<PlatformID>)value;
			Assert.Equal(default(PlatformID), task.Result);
		}

		[Fact]
		public void ProvidesEmptyEnumerable()
		{
			var provider = new EmptyDefaultValueProvider();

			var value = provider.ProvideDefault(typeof(IFoo).GetMethod("IndexesAsync"));

			Assert.IsType<Task<IEnumerable<int>>>(value);

			var task = (Task<IEnumerable<int>>)value;
			Assert.True(task.Result != null && !task.Result.Any());
		}

		[Fact]
		public void ProvidesEmptyArray()
		{
			var provider = new EmptyDefaultValueProvider();

			var value = provider.ProvideDefault(typeof(IFoo).GetMethod("BarsAsync"));

			Assert.IsType<Task<IBar[]>>(value);

			var task = (Task<IBar[]>)value;
			Assert.True(task.Result != null && task.Result.Length == 0);
		}

		[Fact]
		public void ProvidesNullReferenceTypes()
		{
			var provider = new EmptyDefaultValueProvider();

			var value1 = provider.ProvideDefault(typeof(IFoo).GetMethod("BarAsync"));
			var value2 = provider.ProvideDefault(typeof(IFoo).GetMethod("ObjectAsync"));

			Assert.IsType<Task<IBar>>(value1);
			Assert.Null(( (Task<IBar>)value1 ).Result);

			Assert.IsType<Task<object>>(value2);
			Assert.Null(( (Task<object>)value2 ).Result);
		}

		[Fact]
		public void ProvideEmptyQueryable()
		{
			var provider = new EmptyDefaultValueProvider();
			var value = provider.ProvideDefault(typeof(IFoo).GetMethod("QueryableAsync"));

			Assert.IsType<Task<IQueryable<int>>>(value);

			Assert.Equal(0, ( (Task<IQueryable<int>>)value ).Result.Count());
		}

		[Fact]
		public void ProvideEmptyQueryableObjects()
		{
			var provider = new EmptyDefaultValueProvider();
			var value = provider.ProvideDefault(typeof(IFoo).GetMethod("QueryableObjectsAsync"));

			Assert.IsType<Task<IQueryable>>(value);

			Assert.Equal(0, ( (Task<IQueryable>)value ).Result.Cast<object>().Count());
		}

		public interface IFoo
		{
			Task MethodAsync();
			Task<object> ObjectAsync();
			Task<IBar> BarAsync();
			Task<string> StringValueAsync();
			Task<int> IntValueAsync();
			Task<bool> BoolValueAsync();
			Task<int?> NullableIntValueAsync();
			Task<PlatformID> PlatformAsync();
			Task<IEnumerable<int>> IndexesAsync();
			Task<IBar[]> BarsAsync();
			Task<IQueryable<int>> QueryableAsync();
			Task<IQueryable> QueryableObjectsAsync();
		}

		public interface IBar { }
	}
}