#!/usr/bin/env bash
set -euo pipefail

configuration="${1:-Release}"
root="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
results="$root/artifacts/coverage"

rm -rf "$results"
mkdir -p "$results"

dotnet test "$root/TicTacToe.sln"   --configuration "$configuration"   --collect "XPlat Code Coverage"   --settings "$root/tests/coverage.runsettings"   --results-directory "$results"

report="$(find "$results" -name coverage.cobertura.xml -type f | head -n 1)"

if [[ -z "$report" ]]; then
  echo "O relatório coverage.cobertura.xml não foi encontrado." >&2
  exit 1
fi

python3 - "$report" <<'PY'
import sys
import xml.etree.ElementTree as ET

path = sys.argv[1]
root = ET.parse(path).getroot()

print(f"Report: {path}")
print(f"LineRate: {root.attrib.get('line-rate', '0')}")
print(f"BranchRate: {root.attrib.get('branch-rate', '0')}")
print(f"LinesCovered: {root.attrib.get('lines-covered', '0')}")
print(f"LinesValid: {root.attrib.get('lines-valid', '0')}")
print(f"BranchesCovered: {root.attrib.get('branches-covered', '0')}")
print(f"BranchesValid: {root.attrib.get('branches-valid', '0')}")
PY
