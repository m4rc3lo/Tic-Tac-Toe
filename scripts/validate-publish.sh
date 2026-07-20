#!/usr/bin/env bash
set -euo pipefail

publish_directory="${1:?Informe o diretório publicado.}"
publish_directory="$(cd "$publish_directory" && pwd)"

required=(
  "CITATION.cff"
  "README-PUBLISH.md"
  "settings.example.json"
)

missing=()
for file in "${required[@]}"; do
  [[ -f "$publish_directory/$file" ]] || missing+=("$file")
done

forbidden=()
for directory in data exports; do
  [[ ! -d "$publish_directory/$directory" ]] ||
    forbidden+=("$directory/")
done

while IFS= read -r file; do
  forbidden+=("${file#"$publish_directory/"}")
done < <(
  find "$publish_directory" -type f     \( -name settings.json     -o -name matches.json     -o -name statistics.json     -o -name experiment-result.json     -o -name experiment-metrics.csv \)
)

size_bytes="$(find "$publish_directory" -type f -printf '%s\n' |
  awk '{ total += $1 } END { print total + 0 }')"
size_mib="$(awk -v size="$size_bytes"   'BEGIN { printf "%.2f", size / 1024 / 1024 }')"

printf 'Directory: %s\n' "$publish_directory"
printf 'SizeBytes: %s\n' "$size_bytes"
printf 'SizeMiB: %s\n' "$size_mib"
printf 'MissingFiles: %s\n' "${missing[*]:-}"
printf 'ForbiddenEntries: %s\n' "${forbidden[*]:-}"

if (( ${#missing[@]} > 0 || ${#forbidden[@]} > 0 )); then
  exit 1
fi
