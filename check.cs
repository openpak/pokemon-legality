using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using PKHeX.Core;

namespace PKHaX {
	// OpenPak addition: a JSON legality check for any generation, used by the Pokemon Global Link
	// service (GTS deposits) and anything else that needs a verdict on a raw Pokemon file.
	//
	//   POST /v1/check   {"data":"<base64 of the decrypted/stored .pk3-.pk9 bytes>","format":"pk6"}
	//   GET  /healthz
	//
	// "format" is optional (pk3..pk9, pb7, pb8, pa8, pk9...) and only hints at the generation when
	// the length alone is ambiguous. Set PKHAX_INTERNAL_KEY to require "X-Internal-Key" on /v1/*.
	static class Check {
		public sealed class CheckRequest {
			[JsonPropertyName("data")] public string Data { get; set; } = "";
			[JsonPropertyName("format")] public string? Format { get; set; }
		}

		public sealed class CheckResult {
			[JsonPropertyName("valid")] public bool Valid { get; set; }
			[JsonPropertyName("parsed")] public bool Parsed { get; set; }
			[JsonPropertyName("generation")] public int Generation { get; set; }
			[JsonPropertyName("species")] public int Species { get; set; }
			[JsonPropertyName("form")] public int Form { get; set; }
			[JsonPropertyName("level")] public int Level { get; set; }
			[JsonPropertyName("nickname")] public string Nickname { get; set; } = "";
			[JsonPropertyName("ot")] public string OT { get; set; } = "";
			[JsonPropertyName("shiny")] public bool Shiny { get; set; }
			[JsonPropertyName("gender")] public int Gender { get; set; }
			[JsonPropertyName("nature")] public int Nature { get; set; }
			[JsonPropertyName("ability")] public int Ability { get; set; }
			[JsonPropertyName("held_item")] public int HeldItem { get; set; }
			[JsonPropertyName("moves")] public List<int> Moves { get; set; } = new();
			[JsonPropertyName("issues")] public List<string> Issues { get; set; } = new();
			[JsonPropertyName("error")] public string? Error { get; set; }
		}

		static readonly string? InternalKey = Environment.GetEnvironmentVariable("PKHAX_INTERNAL_KEY");

		static EntityContext Hint(string? f) => (f ?? "").ToLowerInvariant().TrimStart('.') switch {
			"pk3" => EntityContext.Gen3,
			"pk4" => EntityContext.Gen4,
			"pk5" => EntityContext.Gen5,
			"pk6" => EntityContext.Gen6,
			"pk7" => EntityContext.Gen7,
			"pb7" => EntityContext.Gen7b,
			"pk8" => EntityContext.Gen8,
			"pa8" => EntityContext.Gen8a,
			"pb8" => EntityContext.Gen8b,
			"pk9" => EntityContext.Gen9,
			_ => EntityContext.Gen9,
		};

		public static bool Authorized(HttpListenerRequest req) =>
			string.IsNullOrEmpty(InternalKey) || req.Headers["X-Internal-Key"] == InternalKey;

		public static (int, string) Handle(HttpListenerRequest req) {
			if (!Authorized(req)) return (401, "{\"error\":\"unauthorized\"}");
			using var sr = new StreamReader(req.InputStream, Encoding.UTF8);
			CheckRequest? body;
			try { body = JsonSerializer.Deserialize<CheckRequest>(sr.ReadToEnd()); }
			catch { return (400, "{\"error\":\"bad json\"}"); }
			if (body == null || string.IsNullOrEmpty(body.Data)) return (400, "{\"error\":\"data required\"}");
			byte[] raw;
			try { raw = Convert.FromBase64String(body.Data); }
			catch { return (400, "{\"error\":\"data is not base64\"}"); }
			return (200, JsonSerializer.Serialize(Run(raw, body.Format)));
		}

		public static CheckResult Run(byte[] raw, string? format) {
			var res = new CheckResult();
			try {
				var pk = EntityFormat.GetFromBytes(raw, Hint(format));
				if (pk == null) { res.Error = "unrecognised Pokemon file length " + raw.Length; return res; }
				var la = new LegalityAnalysis(pk);
				res.Parsed = la.Parsed;
				res.Valid = la.Parsed && la.Valid;
				res.Generation = pk.Generation;
				res.Species = pk.Species;
				res.Form = pk.Form;
				res.Level = pk.CurrentLevel;
				res.Nickname = pk.Nickname;
				res.OT = pk.OriginalTrainerName;
				res.Shiny = pk.IsShiny;
				res.Gender = pk.Gender;
				res.Nature = (int)pk.Nature;
				res.Ability = pk.Ability;
				res.HeldItem = pk.HeldItem;
				foreach (var m in new[] { pk.Move1, pk.Move2, pk.Move3, pk.Move4 }) if (m != 0) res.Moves.Add(m);
				if (!res.Valid)
					foreach (var r in la.Results)
						if (!r.Valid) res.Issues.Add(la.Info.Entity is null ? r.Identifier.ToString() : la.Info.Entity.GetType().Name + ":" + r.Identifier + ":" + r.Judgement);
				if (!res.Valid && res.Issues.Count == 0) res.Issues.Add(la.Parsed ? "illegal" : "unparsed");
			} catch (Exception e) {
				res.Error = e.Message;
			}
			return res;
		}
	}
}
