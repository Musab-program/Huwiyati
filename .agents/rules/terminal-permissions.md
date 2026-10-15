# Rule: Terminal Execution & Command Permissions

## 1. Auto-Approved Read-Only Inspection Commands
The agent is explicitly allowed to execute read-only, non-destructive inspection commands directly without requesting user confirmation, as these commands do not alter or delete any data or codebase state:
- **File System Inspection**: `Get-ChildItem`, `dir`, `ls`
- **Text & Pattern Search**: `Select-String`, `findstr`, `grep`
- **File Viewing**: `Get-Content`, `type`, `cat`
- **Git State Queries**: `git status`, `git log`, `git diff`, `git branch`
- **Tooling Verification**: `dotnet --version`, `dotnet --info`

## 2. Mutating & Destructive Operations Require Prior User Approval
The agent **MUST NOT** execute any command or modification that alters the workspace, database, or environment without presenting the plan and receiving explicit user approval first:
- **Code & File Modifications**: Editing existing code, creating new files, or deleting files (`Remove-Item`, `del`, `rm`).
- **Database & Migration Actions**: `dotnet ef migrations add`, `dotnet ef database update`, or direct SQL execution.
- **Process & Service Alterations**: Stopping or terminating processes, modifying system configurations.
- **Any Other Shell Commands**: Any terminal command not explicitly listed in the auto-approved read-only list above must be proposed to the user first.
