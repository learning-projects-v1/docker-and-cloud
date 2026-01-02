import { Component, OnDestroy, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { HttpService } from './http-service';
import { Subject, take, takeUntil } from 'rxjs';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, FormsModule],
  templateUrl: './app.html',
  styleUrl: './app.scss',
})
export class App implements OnDestroy {
  protected readonly title = signal('project-2-frontend');
  private ngUnsubscribe = new Subject<void>();

  public fileReadResult: string = '';
  public systemInfo: string = '';
  public root: string = '';
  public environmentInfo: string = '';
  public logs: string = '';
  public logMessage: string = "";

  constructor(private httpService: HttpService) {}

  readFromFile() {
    this.httpService
      .getFileReadResult()
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe((res) => {
        if (Array.isArray(res)) {
          res.forEach((x) => {
            this.fileReadResult += JSON.stringify(x);
          });
        } else {
          this.fileReadResult = JSON.stringify(res);
        }
      });
  }

  getSystemInfo() {
    this.httpService
      .getSystemInfo()
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe((res) => {
        this.systemInfo = JSON.stringify(res, null, 2);
      });
  }

  HitRootUrl() {
    this.httpService
      .getRootResult()
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe((res) => {
        this.root = JSON.stringify(res, null, 2);
      });
  }

  getEnvironmentInfo() {
    this.httpService
      .getEnvironmentInfo()
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe((res) => {
        this.environmentInfo = JSON.stringify(res, null, 2);
      });
  }

  postLogs(log: string) {
    this.httpService
      .postLogs(log)
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe((res) => {
        console.log('log posted!');
      });
  }

  getLogs() {
    this.httpService
      .getLogs()
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe((res) => {
        this.logs = JSON.stringify(res, null, 2);
      });
  }

  deleteLogs(){
    this.httpService.deleteLogs()
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe(res => {
        console.log("logs deleted!");
      })
  }

  ngOnDestroy(): void {
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }
}
