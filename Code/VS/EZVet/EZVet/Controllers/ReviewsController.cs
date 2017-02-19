﻿using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using EZVet.DTOs;
using EZVet.Filters;
using EZVet.QueryProcessors;

namespace EZVet.Controllers
{
    public class ReviewsController : ApiController
    {
        private readonly IReviewsQueryProcessor _reviewsQueryProcessor;

        public ReviewsController(IReviewsQueryProcessor ReviewsQueryProcessor)
        {
            _reviewsQueryProcessor = ReviewsQueryProcessor;
        }

        [HttpGet]
        [Route("api/reviews/search")]
        [Authorize(Roles = Consts.Roles.Admin + "," + Consts.Roles.Owner + "," + Consts.Roles.Doctor)]
        public List<Review> Search(int? customerId = null)
        {
            return _reviewsQueryProcessor.Search(customerId ?? 0).ToList();
        }

        [HttpGet]
        [Authorize(Roles = Consts.Roles.Admin + "," + Consts.Roles.Owner + "," + Consts.Roles.Doctor)]
        public Review Get(int id)
        {
            return _reviewsQueryProcessor.GetReview(id);
        }

        [HttpPost]
        [TransactionFilter]
        [Authorize(Roles = Consts.Roles.Admin + "," + Consts.Roles.Doctor)]
        public Review Save([FromBody]Review Review)
        {
            var currPrincipal = HttpContext.Current.User as ClaimsPrincipal;
            var currIdentity = currPrincipal.Identity as BasicAuthenticationIdentity;
            var userId = currIdentity.UserId;

          

            return _reviewsQueryProcessor.Save(Review);
        }

        [HttpPut]
        [TransactionFilter]
        [Authorize(Roles = Consts.Roles.Admin + "," +Consts.Roles.Doctor)]
        public Review Update([FromUri]int id, [FromBody]Review Review)
        {
            return _reviewsQueryProcessor.Update(id, Review);
        }
    }
}