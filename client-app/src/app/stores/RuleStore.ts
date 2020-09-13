import { RootStore } from './rootStore';
import { observable, action } from 'mobx';

export default class RuleStore {
  rootStore: RootStore;
  constructor(rootStore: RootStore) {
    this.rootStore = rootStore;
  }
  @observable activeTab: number = 0;

  @action setActiveTab = (activeIndex: number) => {
      this.activeTab = activeIndex;
  } 
}