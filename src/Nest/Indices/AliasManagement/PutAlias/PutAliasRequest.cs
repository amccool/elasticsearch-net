﻿using Elasticsearch.Net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nest
{

	[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
	public interface IPutAliasRequest : IRequest<PutAliasRequestParameters>
	{
		[JsonProperty("routing")]
		string Routing { get; set; }

		[JsonProperty("filter")]
		[JsonConverter(typeof(CompositeJsonConverter<ReadAsTypeJsonConverter<QueryContainer>, CustomJsonConverter>))]
		IQueryContainer Filter { get; set; }
	}

	public partial class PutAliasRequest : RequestBase<PutAliasRequestParameters>, IPutAliasRequest
	{
		public string Routing { get; set; }

		public IQueryContainer Filter { get; set; }
	}

	[DescriptorFor("IndicesPutAlias")]
	public partial class PutAliasDescriptor 
		: RequestDescriptorBase<PutAliasDescriptor, PutAliasRequestParameters>, IPutAliasRequest
	{
		IPutAliasRequest Self => this;
		string IPutAliasRequest.Routing { get; set; }
		IQueryContainer IPutAliasRequest.Filter { get; set; }
		
		public PutAliasDescriptor Routing(string routing)
		{
			Self.Routing = routing;
			return this;
		}

		public PutAliasDescriptor Filter<T>(Func<QueryContainerDescriptor<T>, QueryContainer> filterSelector)
			where T : class
		{
			filterSelector.ThrowIfNull("filterSelector");
			Self.Filter = filterSelector(new QueryContainerDescriptor<T>());
			return this;
		}
	}
}
