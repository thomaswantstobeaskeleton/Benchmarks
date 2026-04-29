#!/usr/bin/env bash
set -euo pipefail

SDK_VERSION="${1:-8.0.204}"
INSTALL_DIR="${DOTNET_INSTALL_DIR:-$PWD/.dotnet}"
ARTIFACT_PATH="${DOTNET_SDK_TARBALL:-}"

mkdir -p "$INSTALL_DIR"

log(){ echo "[bootstrap-dotnet] $*"; }

install_from_tarball(){
  local tarball="$1"
  if [[ ! -f "$tarball" ]]; then
    log "Tarball not found: $tarball"
    return 1
  fi
  log "Installing SDK from local tarball: $tarball"
  tar -xzf "$tarball" -C "$INSTALL_DIR"
  "$INSTALL_DIR/dotnet" --info
}

if [[ -n "$ARTIFACT_PATH" ]]; then
  install_from_tarball "$ARTIFACT_PATH"
  exit 0
fi

if command -v dotnet >/dev/null 2>&1; then
  log "dotnet already available: $(dotnet --version)"
  exit 0
fi

if [[ -f "$HOME/.dotnet/dotnet" ]]; then
  log "Found existing dotnet in $HOME/.dotnet"
  export PATH="$HOME/.dotnet:$PATH"
  dotnet --info
  exit 0
fi

log "Online installation attempt via dot.net install script"
if command -v curl >/dev/null 2>&1; then
  curl -fsSL https://dot.net/v1/dotnet-install.sh -o /tmp/dotnet-install.sh || true
elif command -v wget >/dev/null 2>&1; then
  wget -O /tmp/dotnet-install.sh https://dot.net/v1/dotnet-install.sh || true
fi

if [[ -f /tmp/dotnet-install.sh ]]; then
  bash /tmp/dotnet-install.sh --version "$SDK_VERSION" --install-dir "$INSTALL_DIR" && "$INSTALL_DIR/dotnet" --info && exit 0
fi

cat <<MSG
No viable online source is reachable from this container.
Workaround started: use offline SDK artifact.
Steps:
  1) Download dotnet-sdk-${SDK_VERSION}-linux-x64.tar.gz on a machine with internet.
  2) Copy it into this repo (for example: /workspace/Benchmarks/WinOpti/.cache/dotnet-sdk.tar.gz).
  3) Run:
       DOTNET_SDK_TARBALL=/workspace/Benchmarks/WinOpti/.cache/dotnet-sdk.tar.gz ./scripts/bootstrap-dotnet.sh ${SDK_VERSION}
MSG
exit 2
