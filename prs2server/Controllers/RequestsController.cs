using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using prs2server.Models;

namespace prs2server.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class RequestsController : ControllerBase {

        public static string StatusNew = "NEW";
        public static string StatusReview = "REVIEW";
        public static string StatusApproved = "APPROVED";
        public static string StatusRejected = "REJECTED";

        private readonly Prs2DbContext _context;

        public RequestsController(Prs2DbContext context) {
            _context = context;
        }

        // GET: api/Requests/reviews/userid
        [HttpGet("reviewed/{userid}")]
        public async Task<ActionResult<IEnumerable<Request>>> GetRequestsInReview(int userid) {
            return await _context.Requests.Where(r => r.Status.Equals(StatusReview) 
                                                        && r.UserId != userid).ToListAsync();
        }

        // PUT: api/Requests/Review
        [HttpPut("review")]
        public async Task<IActionResult> SetRequestToReview(Request request) {
            request.Status = request.Total <= 50 ? StatusApproved : StatusReview;
            return await PutRequest(request.Id, request);
        }

        // PUT: api/Requests/Approved
        [HttpPut("approve")]
        public async Task<IActionResult> SetRequestToApproved(Request request) {
            request.Status = StatusApproved;
            return await PutRequest(request.Id, request);
        }

        // PUT: api/Requests/Rejected
        [HttpPut("reject")]
        public async Task<IActionResult> SetRequestToRejected(Request request) {
            request.Status = StatusRejected;
            return await PutRequest(request.Id, request);
        }

        // GET: api/Requests
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Request>>> GetRequests() {
            return await _context.Requests.ToListAsync();
        }

        // GET: api/Requests/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Request>> GetRequest(int id) {
            var request = await _context.Requests.FindAsync(id);

            if(request == null) {
                return NotFound();
            }

            return request;
        }

        // PUT: api/Requests/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRequest(int id, Request request) {
            if(id != request.Id) {
                return BadRequest();
            }

            _context.Entry(request).State = EntityState.Modified;

            try {
                await _context.SaveChangesAsync();
            } catch(DbUpdateConcurrencyException) {
                if(!RequestExists(id)) {
                    return NotFound();
                } else {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Requests
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Request>> PostRequest(Request request) {
            _context.Requests.Add(request);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRequest", new { id = request.Id }, request);
        }

        // DELETE: api/Requests/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Request>> DeleteRequest(int id) {
            var request = await _context.Requests.FindAsync(id);
            if(request == null) {
                return NotFound();
            }

            _context.Requests.Remove(request);
            await _context.SaveChangesAsync();

            return request;
        }

        private bool RequestExists(int id) {
            return _context.Requests.Any(e => e.Id == id);
        }
    }
}
