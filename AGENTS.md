# Agent Instructions

This project uses **bd** (beads) for issue tracking. Run `bd onboard` to get started.

## Quick Reference

```bash
bd ready              # Find available work
bd show <id>          # View issue details
bd update <id> --status in_progress  # Claim work
bd close <id>         # Complete work
bd sync               # Sync with git
```

## Issue Requirements

Before adding any issue to bd, it MUST have:

- **Acceptance criteria** - Clear definition of what constitutes completion
- **Steps to reproduce or solve** - Detailed steps for reproduction (bugs) or implementation (features)
- **List of files to review** - Specific files that need to be examined or modified

## Documentation Standards

### Diagram Creation

For drawing diagrams use Mermaid in .md files.

Use Mermaid syntax for creating diagrams directly in Markdown files. Examples:

- Architecture diagrams
- Flow charts
- Database schemas
- Sequence diagrams

## Issue Completion Process

**Before closing any issue:**

1. **Request confirmation** - Ask user "Should I close issue #ID?" and wait for affirmative response
2. **Close issue** - Only after getting confirmation, run `bd close <id>`
3. **Commit work** - Make git commit with comprehensive message describing what was accomplished

## Landing the Plane (Session Completion)

**When ending a work session**, you MUST complete ALL steps below. Work is NOT complete until `git push` succeeds.

**MANDATORY WORKFLOW:**

1. **File issues for remaining work** - Create issues for anything that needs follow-up
2. **Run quality gates** (if code changed) - Tests, linters, builds
3. **Update issue status** - Close finished work, update in-progress items
4. **PUSH TO REMOTE** - This is MANDATORY:

   ```bash
   git pull --rebase
   bd sync
   git push
   git status  # MUST show "up to date with origin"
   ```

5. **Clean up** - Clear stashes, prune remote branches
6. **Verify** - All changes committed AND pushed
7. **Hand off** - Provide context for next session

- Work is NOT complete until `git push` succeeds
- NEVER stop before pushing - that leaves work stranded locally
- NEVER say "ready to push when you are" - YOU must push
- If push fails, resolve and retry until it succeeds- NEVER stop before pushing - that leaves work stranded locally
- NEVER say "ready to push when you are" - YOU must push
- If push fails, resolve and retry until it succeeds
