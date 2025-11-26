# Container-Canary

This project is a Proof of Concept demonstrating automated testing of a .NET Core API running in containers and calling a mock server. It is designed to be used in CI/CD pipelines to validate application behavior in isolated, reproducible test environments.

## Architecture

```text
                         +-------------------+
                         |   Test Solution   |
                         | (Integration Tests)
                         |   [Container]     |
                         +---------+---------+
                                   |
                                   v
+-----------------------+   HTTP    +----------------------+
|        Canary API     |---------> |  Name Generator Mock |
| (System Under Test)   | <---------|   Service [Container]|
|      [Container]      |           +----------------------+
+-----------------------+

  All components can run independently in containers.
  Tests call only the API; the backend dependency is swappable.

```

### Components

- Canary API – Simple .NET Core Web API with one endpoint calling the mock server.
- Name Generator Mock Server – Returns predefined responses for consistent integration tests.
- Test Solution – Automated integration tests running against the containerized solution (Canary API and Name Generator Mock Service).
- docker-compose – Orchestrates API, mock server, and test runner locally or in CI.

### Goals

- Demonstrate realistic, container-based API testing.
- Enable fast and consistent integration checks in CI/CD.
- Provide a clean blueprint for automated regression testing.

## Run application local environment

### Canary API

Build and run the Canary-API locally with the following commands root directory of the repository:  
``` docker compose build canary-api ```  

### Name Generator Mock Service

Build and run the NAmeGeneratorMockService locally with the following commands root directory of the repository:  
``` docker compose build name-generator-mockservice ```  

### Whole solution  

``` docker-compose up -d ```

## CI/CD Pipeline

This project includes a **two-stage GitHub Actions pipeline** to automate building, containerizing, and testing the application in isolated containers.

### Pipeline Overview

#### Stage 1: Build Images

- Triggered on push or pull request.
- Steps:
  1. Checkout code.
  2. Build Docker images for:
     - API
     - Mock/Real Service
     - Test Solution
  3. Push images to a registry or save as artifacts for reuse in Stage 2.
- Benefits:
  - Faster feedback on build issues.
  - Reusable images for multiple test runs or environments.

#### Stage 2: Run Integration Tests

- Triggered after Stage 1 completes successfully.
- Steps:

  1. Pull previously built Docker images from registry or artifacts.
  2. Start containers with `docker-compose` (API + Mock/Real Service + Test Solution).
  3. Execute integration tests in the Test Solution container.
  4. Tear down containers after tests complete.
  5. Report results (✅ Success / ❌ Fail).

- Benefits:
  - Tests only run on validated images.
  - Clean separation of build vs test responsibilities.
  - Easily switch between mock and real backend services via configuration.

### Pipeline Flow

```text
       ┌─────────────────────┐
       │  Stage 1: Build     │
       │  Docker Images      │
       │  (API, Mock, Tests) │
       └─────────┬───────────┘
                 │
         Images pushed to registry or stored as artifacts
                 │
                 ▼
       ┌─────────────────────┐
       │  Stage 2: Test      │
       │  Run Integration    │
       │  Containers started │
       │  Test Solution → API│
       └─────────────────────┘
                 │
                 ▼
       ┌──────────────────────┐
       │  Test Results        │
       │  ✅ Success/❌ Fail   │
       └──────────────────────┘
```

### Benefits

- All components are containerized for consistency.
- Tests run in an environment identical to local development or production.
- Enables fast, repeatable CI/CD execution.
- Supports easy switching between mock and real backend services via configuration.
