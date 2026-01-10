import { Component, OnDestroy, OnInit, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { HttpService } from './services/http-service';
import { RequestModel, ResponseModel } from './models/dtos';
import { concatWith, Subject, take, takeUntil } from 'rxjs';
import { RabbitmqConfig, RabbitmqTypeConfig } from './configs/rabbitmq-configs';
import { FormsModule } from '@angular/forms';
import { JsonPipe } from '@angular/common';
import { RandomIdGenerator } from './services/random-id-generator';
import { SignalrService } from './services/signalrService';

@Component({
  selector: 'app-root',
  imports: [FormsModule, JsonPipe],
  templateUrl: './app.html',
  styleUrls: ['./app.scss']
})
export class App implements OnInit, OnDestroy {
  private ngUnsubscribe = new Subject<void>();
  public configs: RabbitmqTypeConfig[] = [];
  public message: string = "msg";
  public responses: ResponseModel[] = [];
  public messageId: string = "";

  constructor(private httpService: HttpService, private randomIdGenerator: RandomIdGenerator, private signalrService: SignalrService) {
    this.configs = RabbitmqConfig.getConfig();
    this.signalrService.start();
    this.configs.forEach(config => {
      config.exchanges.forEach(exchange => {
        exchange.routingKeys.forEach(routingKey => {
          const values = this.getRouteValues(routingKey.routePattern).map(x => this.trimValue(this.trimValue(x, "#"), '*'));
          routingKey.actualRoute = values;
        })
      })
    })
  }

  ngOnInit(): void {
    this.signalrService.responseSubject$.pipe(takeUntil(this.ngUnsubscribe)).subscribe(res => {
      this.onAddResponse(res);
    })
  }

  onSendRequest(type: string, exchangeName: string, routingKey: string) {
    const id = this.randomIdGenerator.getRandomNumber();
    this.messageId = id;

    const requestModel: RequestModel = {
      exchangeType: type,
      exchangeName: exchangeName,
      routingKey: routingKey,
      message: this.message,
      correlationId: id
    }
    //todo: show response on individually under each send <p>
    // this.httpService.postDirectRequest(requestModel).pipe(takeUntil(this.ngUnsubscribe)).subscribe(res => {
    //   console.log("Message sent!");
    // })
    this.httpService.postRequest(requestModel).pipe(takeUntil(this.ngUnsubscribe)).subscribe(res => {
      console.log("Message sent!");
    })
  }

  fetchResponse() {
    this.httpService.getResponse().pipe(takeUntil(this.ngUnsubscribe)).subscribe(res => {
      res.reverse().forEach(res => {
        this.onAddResponse(res);
      })
    })
  }


  getRouteValues(routeParam: string) {
    return routeParam.split('.');
  }

  trimValue(value: string, token: string): string {
    let s = 0, e = value.length;
    while (s < e && value.charCodeAt(s) === token.charCodeAt(0)) s++;
    while (e > s && value.charCodeAt(e - 1) === token.charCodeAt(0)) e--;
    return s === 0 && e === value.length ? value : value.slice(s, e);
  }

  onAddResponse(response: ResponseModel){
    /// previous logic: append at the end and html uses reverse
    // this.responses.push(response);
    // if(this.responses.length > 10){
    //   this.responses = this.responses.slice(this.responses.length-10, this.responses.length);
    // }

    this.responses.unshift(response);
    if(this.responses.length > 10){
      this.responses = this.responses.slice(0, 10);
    }

  }

  ngOnDestroy(): void {
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }
}
