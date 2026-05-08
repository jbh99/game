# Multiplayer Architecture

## Authority model
**Server-authoritative** for: unit positions (post-lockstep), HP, ownership, region control, resources.
**Client-only**: visual FX (explosions, muzzle flash, FPS muzzle flash), UI sound, possession camera animation.

## Tick model
- **Lockstep tick rate:** 10 Hz
- **Input delay:** 3 ticks (300 ms)
- Players send commands (move, attack, produce, possess); commands enqueue with `tickToApply = currentTick + 3`.
- All clients apply commands deterministically at the same tick → same world state.

## Why lockstep here
A single mid-game battle can field 600+ units. Replicating positions per-frame at 60 Hz × 600 = 36k position updates/sec, untenable for consumer broadband. Lockstep replicates ~10 commands/sec/player.

## NGO + UGS layer
- **Unity Gaming Services Lobby** for matchmaking metadata (mode, faction, ready state).
- **Unity Relay** for NAT-traversal so player-hosted sessions don't expose IPs.
- **NGO** for the actual command channel (ServerRpc → ClientRpc broadcast).

## Anti-cheat (server-side validation)
- Velocity caps on lockstep `move` commands (reject teleport).
- Resource cost validated server-side before `produce` is broadcast.
- Possession requires unit ownership matches client ID.

## Compile guards
- `UGS` define gates the lobby/relay code paths.
- `NGO` define gates the netcode code paths.
- The project compiles cleanly without either; multiplayer simply logs warnings until installed.
