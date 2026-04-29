#!/usr/bin/env bash
set -euo pipefail
ROOT_DIR="$(cd "$(dirname "$0")/.." && pwd)"
cd "$ROOT_DIR"

if ! command -v dotnet >/dev/null 2>&1; then
  if [[ -x "$ROOT_DIR/.dotnet/dotnet" ]]; then
    export PATH="$ROOT_DIR/.dotnet:$PATH"
  else
    ./scripts/bootstrap-dotnet.sh 8.0.204
    export PATH="$ROOT_DIR/.dotnet:$PATH"
  fi
fi

dotnet --info
dotnet build WinOpti.sln
dotnet test WinOpti.sln
