﻿using System;
using System.Collections.Generic;
using System.Linq;
using Elasticsearch.Net;
using Newtonsoft.Json;
using System.Linq.Expressions;

namespace Nest
{
	public interface ITermVectorsRequest : IRequest<TermVectorsRequestParameters>
	{
		/// <summary>
		/// An optional document to get termvectors for instead of using an already indexed document
		/// </summary>
		[JsonProperty("doc")]
		object Document { get; set; }

		[JsonProperty("per_field_analyzer")]
		IDictionary<FieldName, string> PerFieldAnalyzer { get; set; }
	}

	public interface ITermVectorsRequest<T> : ITermVectorsRequest where T : class { }

	internal static class TermVectorsPathInfo
	{
		public static void Update(IConnectionSettingsValues settings, RequestPath<TermVectorsRequestParameters> pathInfo, ITermVectorsRequest request)
		{
			pathInfo.HttpMethod = request.Document == null ? HttpMethod.GET : HttpMethod.POST;
		}
	}

	public partial class TermVectorsRequest : RequestBase<TermVectorsRequestParameters>, ITermVectorsRequest
	{
		protected override void UpdateRequestPath(IConnectionSettingsValues settings, RequestPath<TermVectorsRequestParameters> pathInfo)
		{
			TermVectorsPathInfo.Update(settings, pathInfo, this);
		}

		public object Document { get; set; }

		public IDictionary<FieldName, string> PerFieldAnalyzer { get; set; }
	}

	public partial class TermVectorsRequest<T> : RequestBase<TermVectorsRequestParameters>, ITermVectorsRequest<T>
		where T : class
	{
		object ITermVectorsRequest.Document { get; set; }

		IDictionary<FieldName, string> ITermVectorsRequest.PerFieldAnalyzer { get; set; }

		protected override void UpdateRequestPath(IConnectionSettingsValues settings, RequestPath<TermVectorsRequestParameters> pathInfo)
		{
			TermVectorsPathInfo.Update(settings, pathInfo, this);
		}
	}

	[DescriptorFor("Termvectors")]
	public partial class TermVectorsDescriptor<T> : RequestDescriptorBase<TermVectorsDescriptor<T>, TermVectorsRequestParameters>
		, ITermVectorsRequest
		where T : class
	{
		private ITermVectorsRequest Self => this;
		
		object ITermVectorsRequest.Document { get; set; }

		IDictionary<FieldName, string> ITermVectorsRequest.PerFieldAnalyzer { get; set; }

		public TermVectorsDescriptor<T> Document<TDocument>(TDocument document) where TDocument : class
		{
			Self.Document = document;	
			return this;
		}
		public TermVectorsDescriptor<T> PerFieldAnalyzer(Func<FluentDictionary<Expression<Func<T, object>>, string>, FluentDictionary<Expression<Func<T, object>>, string>> analyzerSelector)
		{
			var d = new FluentDictionary<Expression<Func<T, object>>, string>();
			analyzerSelector(d);
			Self.PerFieldAnalyzer = d.ToDictionary(x => FieldName.Create(x.Key), x => x.Value);
			return this;
		}

		public TermVectorsDescriptor<T> PerFieldAnalyzer(Func<FluentDictionary<FieldName, string>, FluentDictionary<FieldName, string>> analyzerSelector)
		{
			Self.PerFieldAnalyzer = analyzerSelector(new FluentDictionary<FieldName, string>());
			return this;
		}
		protected override void UpdateRequestPath(IConnectionSettingsValues settings, RequestPath<TermVectorsRequestParameters> pathInfo)
		{
			TermVectorsPathInfo.Update(settings, pathInfo, this);
		}
	}
}
