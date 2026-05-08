# Game Design Document — AI Hegemony Wars

## Pillars (non-negotiable)
1. **Real LLM data is the gameplay**, not a skin. Every unit's stats trace back to a published benchmark.
2. **Top-down command + first-person possession** are inseparable. Player agency must matter.
3. **The model market is a clock.** Version drift creates emergent late-game tension without scripted plot beats.

## Core loops
| Window | Player verb | Reward |
|---|---|---|
| Moment-to-moment (0–30 s) | Possess unit, fire weapons, micro a flank | Visible kills, +50% buff confirmation |
| Session (5–30 min) | Capture region, swap roster, win battle | Resource yield + roster upgrade |
| Long-term (hours) | Survive forced version drift, win continent | Faction tech-tree progression |

## Action Value formula
```
AV = 10000 / tokensPerSecond
```
Lower AV ⇒ unit's Action Gauge fills faster ⇒ faster spawn cadence and attack rate.

## Possession buff
When a player possesses a unit:
- Move speed × 1.5
- Damage × 1.5
- NavMeshAgent disabled, manual WASD + mouse aim active
- 1st-person camera anchored at `headBone`

## Regional buffs
| Faction | Home region tag | Production | Damage | Cost |
|---|---|---|---|---|
| USA | `AmericasContinent` | +20% | +40% | × 1.0 |
| China | `AsiaContinent` | +30% | × 1.0 | -40% |

## Version drift events
- **Claude Opus 4.6 hallucination** — unit ignores commands and fires at random points for 60 s
- **GPT-5.4 throttling** — unit's action gauge ticks at 40% rate for 45 s

## Win conditions
- Single player: control 4 of 6 continents OR eliminate all enemy faction headquarters
- Multiplayer (FFA AI vs AI): last faction with ≥1 production facility standing

## Open questions for next iteration
- Tutorial pacing on Korean Peninsula — does the C/V toggle introduce naturally inside 90 s?
- Late-game balance: GPT-5.5 vs DeepSeek V4 Pro under combined home buffs
