#!/usr/bin/env python3
import csv
import hashlib
import json
import pathlib
import statistics
import sys


def fail(message: str) -> None:
    raise SystemExit(f"Erro de validação: {message}")


root = pathlib.Path(
    sys.argv[1]
    if len(sys.argv) > 1
    else "artifacts/experiments/reference"
)
manifest_path = root / "reference-manifest.json"

if not manifest_path.is_file():
    fail(f"manifesto ausente: {manifest_path}")

manifest = json.loads(manifest_path.read_text(encoding="utf-8"))
required = [
    "experiment_id",
    "run_number",
    "x_strategy",
    "o_strategy",
    "seed",
    "result",
    "move_count",
    "duration_ms",
    "evaluated_states",
    "failed",
    "failure_message",
    "application_version",
]
rows = []

for scenario in manifest["scenarios"]:
    scenario_id = scenario["scenarioId"]
    folder = root / scenario["directory"]
    csv_path = folder / scenario["csvFile"]
    json_path = folder / scenario["jsonFile"]

    if not csv_path.is_file():
        fail(f"{scenario_id}: CSV ausente: {csv_path}")

    if not json_path.is_file():
        fail(f"{scenario_id}: JSON ausente: {json_path}")

    csv_hash = hashlib.sha256(csv_path.read_bytes()).hexdigest()
    json_hash = hashlib.sha256(json_path.read_bytes()).hexdigest()

    if csv_hash != scenario["csvSha256"]:
        fail(
            f"{scenario_id}: hash CSV divergente; "
            f"manifesto={scenario['csvSha256']}, atual={csv_hash}"
        )

    if json_hash != scenario["jsonSha256"]:
        fail(
            f"{scenario_id}: hash JSON divergente; "
            f"manifesto={scenario['jsonSha256']}, atual={json_hash}"
        )

    data = json.loads(json_path.read_text(encoding="utf-8"))

    with csv_path.open(encoding="utf-8", newline="") as file:
        reader = csv.DictReader(file, delimiter=";")

        if reader.fieldnames != required:
            fail(
                f"{scenario_id}: cabeçalho CSV inválido; "
                f"esperado={required}, atual={reader.fieldnames}"
            )

        scenario_rows = list(reader)

    summary_count = data["summary"]["totalRuns"]
    metric_count = len(data["metrics"])
    csv_count = len(scenario_rows)

    if not (
        summary_count == metric_count == csv_count
    ):
        fail(
            f"{scenario_id}: contagens inconsistentes; "
            f"summary.totalRuns={summary_count}, "
            f"metrics={metric_count}, "
            f"csvRows={csv_count}. "
            "Apague o diretório do cenário e execute novamente."
        )

    for row in scenario_rows:
        row["scenario_id"] = scenario_id

    rows.extend(scenario_rows)

if not rows:
    fail("nenhuma métrica foi encontrada")

successful = [
    row
    for row in rows
    if row["failed"].lower() == "false"
]

summary = {
    "totalRuns": len(rows),
    "failedRuns": sum(
        row["failed"].lower() == "true"
        for row in rows
    ),
    "results": {},
}

for result in ("XWins", "OWins", "Draw", "Failed"):
    summary["results"][result] = sum(
        row["result"] == result
        for row in rows
    )

summary["averageMoves"] = (
    statistics.fmean(
        int(row["move_count"])
        for row in successful
    )
    if successful
    else 0
)
summary["averageDurationMs"] = (
    statistics.fmean(
        int(row["duration_ms"])
        for row in successful
    )
    if successful
    else 0
)

(root / "reference-summary.json").write_text(
    json.dumps(summary, indent=2),
    encoding="utf-8",
)

counts = summary["results"]
width, height = 700, 360
max_value = max(counts.values()) or 1
bars = []

for index, (label, value) in enumerate(counts.items()):
    x = 90 + index * 145
    bar_height = 220 * value / max_value
    y = 290 - bar_height
    bars.append(
        f'<rect x="{x}" y="{y:.1f}" width="80" '
        f'height="{bar_height:.1f}" fill="#0786b4"/>'
        f'<text x="{x + 40}" y="315" '
        f'text-anchor="middle">{label}</text>'
        f'<text x="{x + 40}" y="{y - 8:.1f}" '
        f'text-anchor="middle">{value}</text>'
    )

svg = (
    f'<svg xmlns="http://www.w3.org/2000/svg" '
    f'width="{width}" height="{height}">'
    '<rect width="100%" height="100%" fill="white"/>'
    '<text x="350" y="30" text-anchor="middle" font-size="20">'
    'Resultados do experimento de referência'
    '</text>'
    '<line x1="60" y1="290" x2="660" y2="290" stroke="black"/>'
    f'{"".join(bars)}'
    '</svg>'
)

(root / "reference-results.svg").write_text(
    svg,
    encoding="utf-8",
)

print(json.dumps(summary, indent=2))
